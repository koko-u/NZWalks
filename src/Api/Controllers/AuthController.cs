using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NZWalk.Api.Extensions;
using NZWalk.Api.Responses;
using NZWalks.Core.Features.Auth.Dto;
using NZWalks.Core.Features.Auth.Result;
using NZWalks.Core.Features.Auth.Services;
using NZWalks.Core.Features.Auth.Settings;

namespace NZWalk.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(
    AuthService authService,
    IOptions<JwtOptions> options,
    ILogger<AuthController> logger
) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register(
        [FromBody] RegisterUserDto registerUserDto,
        [FromServices] IValidator<RegisterUserDto> validator,
        CancellationToken ct
    )
    {
        var result = await validator.ValidateAsync(registerUserDto, ct);
        if (!result.IsValid)
        {
            ModelState.Apply(result.Errors);
            return ValidationProblem(ModelState);
        }

        var user = await authService.RegisterAsync(registerUserDto, ct);
        logger.LogInformation("Successfully Logged In: {0}", user.Email);

        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponse>> Login(
        LoginUserDto loginUserDto,
        CancellationToken ct
    )
    {
        var authResult = await authService.LoginAsync(loginUserDto, ct);
        switch (authResult)
        {
            case AuthResult.InvalidCredentials:
                return Unauthorized();
            case AuthResult.Authenticated auth:
                var response = new AuthResponse
                {
                    AccessToken = auth.AccessToken,
                    RefreshToken = auth.RefreshToken,
                    ExpiresIn = options.Value.AccessTokenExpirationMinutes * 60,
                };
                return Ok(response);
            default:
                throw new UnreachableException();
        }
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Refresh(
        [FromBody] RefreshDto refreshDto,
        CancellationToken ct
    )
    {
        var refreshResult = await authService.RotateRefreshToken(refreshDto, ct);
        switch (refreshResult)
        {
            case RefreshResult.Invalid:
            case RefreshResult.Expired:
                return Unauthorized();
            case RefreshResult.Success success:
                var response = new AuthResponse
                {
                    AccessToken = success.AccessToken,
                    RefreshToken = success.RefreshToken,
                    ExpiresIn = options.Value.AccessTokenExpirationMinutes * 60,
                };
                return Ok(response);
            default:
                throw new UnreachableException();
        }
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout(
        [FromBody] LogoutUserDto logoutUserDto,
        CancellationToken ct
    )
    {
        await authService.RevokeRefreshTokenAsync(logoutUserDto, ct);

        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType<MeResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<MeResponse> GetMe()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId))
        {
            return Unauthorized();
        }

        return Ok(
            new MeResponse
            {
                UserId = userId,
                Email = User.FindFirstValue(ClaimTypes.Email),
                Username = User.FindFirstValue(ClaimTypes.Name),
                Roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList(),
            }
        );
    }
}
