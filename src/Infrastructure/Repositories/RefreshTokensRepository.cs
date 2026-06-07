using System;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using Dapper;
using KozLibraries.DapperSqlHelper;
using NZWalk.Infrastructure.Mappers;
using NZWalk.Infrastructure.Rows;
using NZWalks.Core.Features.Auth.Models;
using NZWalks.Core.Features.Auth.Repositories;
using NZWalks.Core.Shared.Tx;

namespace NZWalk.Infrastructure.Repositories;

[AutoRegisterService]
public sealed class RefreshTokensRepository(SqlResource sql) : IRefreshTokensRepository
{
    public Func<DbSession, CancellationToken, Task> ExpireAsync(Guid userId)
    {
        return async (session, ct) =>
        {
            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                commandText: await sql.GetAsync("refresh_tokens/update_expired.sql", ct),
                parameters: new { UserId = userId },
                transaction: tx,
                cancellationToken: ct
            );
            await conn.ExecuteAsync(cmd);
        };
    }

    public Func<DbSession, CancellationToken, Task> CreateAsync(Guid userId, string hashedToken)
    {
        return async (session, ct) =>
        {
            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                commandText: await sql.GetAsync("refresh_tokens/insert_one.sql", ct),
                parameters: new { UserId = userId, TokenHash = hashedToken },
                transaction: tx,
                cancellationToken: ct
            );
            await conn.ExecuteAsync(cmd);
        };
    }

    public Func<DbSession, CancellationToken, Task> RevokeAsync(string hashedToken)
    {
        return async (session, ct) =>
        {
            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                commandText: await sql.GetAsync("refresh_tokens/revoke_all.sql", ct),
                parameters: new { TokenHash = hashedToken },
                transaction: tx,
                cancellationToken: ct
            );
            await conn.ExecuteAsync(cmd);
        };
    }

    public Func<DbSession, CancellationToken, Task<RefreshToken?>> GetByTokenAsync(
        string hashedToken
    )
    {
        return async (session, ct) =>
        {
            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                commandText: await sql.GetAsync("refresh_tokens/select_by_token.sql", ct),
                parameters: new { TokenHash = hashedToken },
                transaction: tx,
                cancellationToken: ct
            );

            var row = await conn.QuerySingleOrDefaultAsync<RefreshTokenRow>(cmd);
            return row?.ToRefreshTokenModel();
        };
    }
}
