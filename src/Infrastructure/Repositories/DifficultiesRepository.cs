using System;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using Dapper;
using NZWalk.Infrastructure.Rows;
using NZWalks.Core.Features.Difficulties.Models;
using NZWalks.Core.Features.Difficulties.Repositories;
using NZWalks.Core.Shared.Tx;

namespace NZWalk.Infrastructure.Repositories;

[AutoRegisterService]
public sealed class DifficultiesRepository : IDifficultiesRepository
{
    public Func<DbSession, CancellationToken, Task<Difficulty?>> GetRegionByNameAsync(string name)
    {
        return async (session, ct) =>
        {
            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                """
                SELECT "id", "name" FROM "difficulties" WHERE "name" = @Name
                """,
                new { Name = name },
                transaction: tx,
                cancellationToken: ct
            );
            var row = await conn.QuerySingleOrDefaultAsync<DifficultyRow>(cmd);
            if (row is null)
            {
                return null;
            }

            return new Difficulty(row.Id, row.Name);
        };
    }
}
