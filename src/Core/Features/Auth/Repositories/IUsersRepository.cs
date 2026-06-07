using System;
using System.Threading;
using System.Threading.Tasks;
using NZWalks.Core.Features.Auth.Dto;
using NZWalks.Core.Features.Auth.Models;
using NZWalks.Core.Shared.Tx;

namespace NZWalks.Core.Features.Auth.Repositories;

public interface IUsersRepository
{
    Func<DbSession, CancellationToken, Task<User>> CreateUserAsync(NewUserDto userDto);

    Func<DbSession, CancellationToken, Task<UserCredentials?>> GetUserCredentialsByEmailAsync(
        string email
    );

    Func<DbSession, CancellationToken, Task> UpdatePasswordHashAsync(
        Guid userId,
        string passwordHash
    );

    Func<DbSession, CancellationToken, Task<User?>> GetUserByIdAsync(Guid id);
}
