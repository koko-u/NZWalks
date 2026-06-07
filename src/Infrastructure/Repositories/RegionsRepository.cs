using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using Dapper;
using KozLibraries.DapperSqlHelper;
using NZWalk.Infrastructure.Mappers;
using NZWalk.Infrastructure.Rows;
using NZWalks.Core.Features.Regions.Dto;
using NZWalks.Core.Features.Regions.Models;
using NZWalks.Core.Features.Regions.Repositories;
using NZWalks.Core.Shared.Tx;

namespace NZWalk.Infrastructure.Repositories;

[AutoRegisterService]
public sealed class RegionsRepository(SqlResource sql) : IRegionsRepository
{
    public Func<DbSession, CancellationToken, Task<IEnumerable<Region>>> GetAllRegionsAsync()
    {
        return async (session, ct) =>
        {
            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                commandText: await sql.GetAsync("regions/select_all.sql", ct),
                transaction: tx,
                cancellationToken: ct
            );

            var rows = await conn.QueryAsync<RegionRow>(cmd);
            return rows.Select(r => r.ToRegionModel());
        };
    }

    public Func<DbSession, CancellationToken, Task<Region?>> GetRegionByIdAsync(Guid id)
    {
        return async (session, ct) =>
        {
            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                commandText: await sql.GetAsync("regions/select_by_id.sql", ct),
                parameters: new { RegionId = id },
                transaction: tx,
                cancellationToken: ct
            );

            var row = await conn.QuerySingleOrDefaultAsync<RegionRow>(cmd);
            return row?.ToRegionModel();
        };
    }

    public Func<DbSession, CancellationToken, Task<Region?>> GetRegionByCodeAsync(string code)
    {
        return async (session, ct) =>
        {
            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                commandText: await sql.GetAsync("regions/select_by_code.sql", ct),
                parameters: new { RegionCode = code },
                transaction: tx,
                cancellationToken: ct
            );

            var row = await conn.QuerySingleOrDefaultAsync<RegionRow>(cmd);
            return row?.ToRegionModel();
        };
    }

    public Func<DbSession, CancellationToken, Task<Region>> CreateRegionAsync(
        NewRegionDto newRegionDto
    )
    {
        return async (session, ct) =>
        {
            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                commandText: await sql.GetAsync("regions/insert_one.sql", ct),
                parameters: new
                {
                    RegionCode = newRegionDto.Code,
                    RegionName = newRegionDto.Name,
                    RegionImageUrl = newRegionDto.ImageUrl,
                },
                transaction: tx,
                cancellationToken: ct
            );

            var row = await conn.QuerySingleAsync<RegionRow>(cmd);
            return row.ToRegionModel();
        };
    }

    public Func<DbSession, CancellationToken, Task<Region?>> UpdateRegionAsync(
        Guid id,
        UpdateRegionDto updateRegionDto
    )
    {
        return async (session, ct) =>
        {
            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                commandText: await sql.GetAsync("regions/update_one.sql", ct),
                parameters: new
                {
                    RegionId = id,
                    RegionCode = updateRegionDto.Code,
                    RegionName = updateRegionDto.Name,
                    RegionImageUrl = updateRegionDto.ImageUrl,
                },
                transaction: tx,
                cancellationToken: ct
            );

            var row = await conn.QuerySingleOrDefaultAsync<RegionRow>(cmd);
            return row?.ToRegionModel();
        };
    }

    public Func<DbSession, CancellationToken, Task<Region?>> DeleteRegionAsync(Guid id)
    {
        return async (session, ct) =>
        {
            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                commandText: await sql.GetAsync("regions/delete_by_id.sql", ct),
                parameters: new { RegionId = id },
                transaction: tx,
                cancellationToken: ct
            );

            var row = await conn.QuerySingleOrDefaultAsync<RegionRow>(cmd);
            return row?.ToRegionModel();
        };
    }
}
