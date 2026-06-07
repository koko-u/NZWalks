using System;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using Dapper;
using KozLibraries.DapperSqlHelper;
using NZWalk.Infrastructure.Mappers;
using NZWalk.Infrastructure.Rows;
using NZWalks.Core.Features.Auth.Dto;
using NZWalks.Core.Features.Auth.Models;
using NZWalks.Core.Features.Auth.Repositories;
using NZWalks.Core.Shared.Tx;

namespace NZWalk.Infrastructure.Repositories;

[AutoRegisterService]
public sealed class UsersRepository(SqlResource sql) : IUsersRepository
{
    public Func<DbSession, CancellationToken, Task<User>> CreateUserAsync(NewUserDto userDto)
    {
        return async (session, ct) =>
        {
            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                commandText: await sql.GetAsync("users/insert_one.sql", ct),
                parameters: new
                {
                    Email = userDto.Email,
                    DisplayName = userDto.DisplayName,
                    PasswordHash = userDto.PasswordHash,
                },
                transaction: tx,
                cancellationToken: ct
            );

            var row = await conn.QuerySingleAsync<UserRow>(cmd);
            return row.ToUserModel();
        };
    }

    public Func<
        DbSession,
        CancellationToken,
        Task<UserCredentials?>
    > GetUserCredentialsByEmailAsync(string email)
    {
        return async (session, ct) =>
        {
            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                commandText: await sql.GetAsync("users/select_credentials_by_email.sql", ct),
                parameters: new { Email = email },
                transaction: tx,
                cancellationToken: ct
            );

            var row = await conn.QuerySingleOrDefaultAsync<UserCredentialsRow>(cmd);
            return row?.ToUserCredentialModel();
        };
    }

    public Func<DbSession, CancellationToken, Task> UpdatePasswordHashAsync(
        Guid userId,
        string passwordHash
    )
    {
        return async (session, ct) =>
        {
            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                commandText: await sql.GetAsync("users/update_password_hash.sql", ct),
                parameters: new { UserId = userId, PasswordHash = passwordHash },
                transaction: tx,
                cancellationToken: ct
            );

            await conn.ExecuteAsync(cmd);
        };
    }

    public Func<DbSession, CancellationToken, Task<User?>> GetUserByIdAsync(Guid id)
    {
        return async (session, ct) =>
        {
            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                commandText: await sql.GetAsync("users/select_by_id.sql", ct),
                parameters: new { Id = id },
                transaction: tx,
                cancellationToken: ct
            );

            var row = await conn.QuerySingleOrDefaultAsync<UserRow>(cmd);
            return row?.ToUserModel();
        };
    }
}
