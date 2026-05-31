using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using Dapper;
using KozLibraries.DapperSqlHelper;
using NZWalk.Infrastructure.Mappers;
using NZWalk.Infrastructure.Rows;
using NZWalks.Core.Models;
using NZWalks.Core.Repositories;
using NZWalks.Core.Tx;

namespace NZWalk.Infrastructure.Repositories;

[AutoRegisterService]
public sealed class WalksRepository(SqlResource sql) : IWalksRepository
{
    public async Task<IEnumerable<Walk>> GetAllWalksAsync(DbSession session, CancellationToken ct)
    {
        var (conn, tx) = session;
        var cmd = new CommandDefinition(
            commandText: await sql.GetAsync("walks/select_all.sql", ct),
            transaction: tx,
            cancellationToken: ct
        );
        var rows = await conn.QueryAsync<WalkRow>(cmd);
        return rows.Select(r =>
        {
            var region = new Region(
                Id: r.RegionId,
                Code: r.RegionCode,
                Name: r.RegionName,
                ImageUrl: r.RegionImageUrl
            );
            var difficulty = new Difficulty(Id: r.DifficultyId, Name: r.DifficultyName);
            var walk = r.ToWalkModel(region, difficulty);

            return walk;
        });
    }
}
