using System;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using Microsoft.AspNetCore.Identity;
using NZWalks.Core.Features.Auth.Dto;
using NZWalks.Core.Features.Auth.Mappers;
using NZWalks.Core.Features.Auth.Models;
using NZWalks.Core.Features.Auth.Repositories;
using NZWalks.Core.Features.Auth.Result;
using NZWalks.Core.Shared.Tx;

namespace NZWalks.Core.Features.Auth.Services;

[AutoRegisterService]
public sealed class AuthService(
    TxRunner txRunner,
    IPasswordHasher<User> passwordHasher,
    IUsersRepository usersRepository,
    IRefreshTokensRepository refreshTokensRepository,
    JwtTokenService jwtTokenService,
    RefreshTokenGenerator refreshTokenGenerator
)
{
    public async Task<User> RegisterAsync(RegisterUserDto registerUserDto, CancellationToken ct)
    {
        var userDto = ComputePasswordHash(registerUserDto);
        return await txRunner.ExecuteAsync(usersRepository.CreateUserAsync(userDto), ct);
    }

    private NewUserDto ComputePasswordHash(RegisterUserDto registerUserDto)
    {
        ArgumentNullException.ThrowIfNull(
            registerUserDto.Password,
            nameof(registerUserDto.Password)
        );
        ArgumentNullException.ThrowIfNull(registerUserDto.Email, nameof(registerUserDto.Email));

        var emptyUser = new User { Id = Guid.Empty, Email = string.Empty };
        var passwordHash = passwordHasher.HashPassword(emptyUser, registerUserDto.Password);

        return new NewUserDto
        {
            Email = registerUserDto.Email,
            DisplayName = registerUserDto.DisplayName,
            PasswordHash = passwordHash,
        };
    }

    public async Task<AuthResult> LoginAsync(LoginUserDto loginUserDto, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(loginUserDto.Password, nameof(loginUserDto.Password));
        ArgumentNullException.ThrowIfNull(loginUserDto.Email, nameof(loginUserDto.Email));

        return await txRunner.ExecuteAsync<AuthResult>(
            async (session, ctn) =>
            {
                // check user credentials
                var userCredentials = await usersRepository.GetUserCredentialsByEmailAsync(
                    loginUserDto.Email
                )(session, ctn);
                if (userCredentials is null)
                {
                    return new AuthResult.InvalidCredentials();
                }
                var verified = passwordHasher.VerifyHashedPassword(
                    userCredentials.ToUser(),
                    userCredentials.PasswordHash,
                    loginUserDto.Password
                );
                switch (verified)
                {
                    case PasswordVerificationResult.Failed:
                        return new AuthResult.InvalidCredentials();
                    case PasswordVerificationResult.SuccessRehashNeeded:
                    {
                        var updatedPasswordHash = passwordHasher.HashPassword(
                            userCredentials.ToUser(),
                            loginUserDto.Password
                        );
                        await usersRepository.UpdatePasswordHashAsync(
                            userCredentials.Id,
                            updatedPasswordHash
                        )(session, ctn);
                        break;
                    }
                    case PasswordVerificationResult.Success:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                // expire old refresh tokens
                await refreshTokensRepository.ExpireAsync(userCredentials.Id)(session, ctn);
                // create new refresh token
                var refreshToken = refreshTokenGenerator.Generate();
                await refreshTokensRepository.CreateAsync(
                    userCredentials.Id,
                    refreshTokenGenerator.Hash(refreshToken)
                )(session, ctn);

                // create JWT token
                var jwtToken = jwtTokenService.GenerateAccessToken(userCredentials.ToUser());

                return new AuthResult.Authenticated(userCredentials.Id, jwtToken, refreshToken);
            },
            ct
        );
    }

    public Task RevokeRefreshTokenAsync(LogoutUserDto logoutUserDto, CancellationToken ct)
    {
        var hashedToken = refreshTokenGenerator.Hash(logoutUserDto.RefreshToken);
        return txRunner.ExecuteAsync(refreshTokensRepository.RevokeAsync(hashedToken), ct);
    }

    public Task<RefreshResult> RotateRefreshToken(RefreshDto refreshDto, CancellationToken ct)
    {
        var hashedToken = refreshTokenGenerator.Hash(refreshDto.RefreshToken);
        return txRunner.ExecuteAsync<RefreshResult>(
            async (session, ctn) =>
            {
                // check refresh token
                var refreshToken = await refreshTokensRepository.GetByTokenAsync(hashedToken)(
                    session,
                    ctn
                );
                if (refreshToken is null)
                {
                    return new RefreshResult.Invalid();
                }

                if (refreshToken.ExpiresAt <= DateTime.UtcNow)
                {
                    return new RefreshResult.Expired();
                }

                // generate new JWT token
                var user = await usersRepository.GetUserByIdAsync(refreshToken.UserId)(
                    session,
                    ctn
                );
                if (user is null)
                {
                    return new RefreshResult.Invalid();
                }

                var jwtToken = jwtTokenService.GenerateAccessToken(user);

                // revoke old refresh token
                await refreshTokensRepository.ExpireAsync(refreshToken.UserId)(session, ctn);
                // create new refresh token
                var newRefreshToken = refreshTokenGenerator.Generate();
                await refreshTokensRepository.CreateAsync(
                    refreshToken.UserId,
                    refreshTokenGenerator.Hash(newRefreshToken)
                )(session, ctn);

                return new RefreshResult.Success(jwtToken, newRefreshToken);
            },
            ct
        );
    }
}
