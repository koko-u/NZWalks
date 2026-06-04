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
using NZWalks.Core.Dto;
using NZWalks.Core.Models;
using NZWalks.Core.Repositories;
using NZWalks.Core.Tx;

namespace NZWalk.Infrastructure.Repositories;

[AutoRegisterService]
public sealed class WalksRepository(SqlResource sql) : IWalksRepository
{
    public Func<DbSession, CancellationToken, Task<IEnumerable<Walk>>> GetAllWalksAsync()
    {
        return async (session, ct) =>
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
        };
    }

    public Func<DbSession, CancellationToken, Task<Walk?>> GetWalkByIdAsync(Guid id)
    {
        return async (session, ct) =>
        {
            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                commandText: await sql.GetAsync("walks/select_by_id.sql", ct),
                parameters: new { Id = id },
                transaction: tx,
                cancellationToken: ct
            );

            var result = await conn.QuerySingleOrDefaultAsync<WalkRow>(cmd);
            return result?.ToWalkModel();
        };
    }

    public Func<DbSession, CancellationToken, Task<Walk>> CreateWalkAsync(NewWalkDto walkDto)
    {
        return async (session, ct) =>
        {
            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                commandText: await sql.GetAsync("walks/insert_one.sql", ct),
                parameters: new
                {
                    RegionCode = walkDto.RegionCode,
                    DifficultyName = walkDto.Difficulty,
                    Name = walkDto.Name,
                    Description = walkDto.Description,
                    LengthKm = walkDto.LengthKm,
                    ImageUrl = walkDto.ImageUrl,
                },
                transaction: tx,
                cancellationToken: ct
            );
            var row = await conn.QuerySingleAsync<WalkRow>(cmd);

            var region = new Region(
                Id: row.RegionId,
                Code: row.RegionCode,
                Name: row.RegionName,
                ImageUrl: row.RegionImageUrl
            );
            var difficulty = new Difficulty(Id: row.DifficultyId, Name: row.DifficultyName);
            var walk = row.ToWalkModel(region, difficulty);

            return walk;
        };
    }

    public Func<DbSession, CancellationToken, Task<Walk?>> UpdateWalkAsync(
        Guid id,
        UpdateWalkDto walkDto
    )
    {
        return async (session, ct) =>
        {
            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                commandText: await sql.GetAsync("walks/update_one.sql", ct),
                parameters: new
                {
                    Id = id,
                    Name = walkDto.Name,
                    Description = walkDto.Description,
                    LengthKm = walkDto.LengthKm,
                    ImageUrl = walkDto.ImageUrl,
                    RegionCode = walkDto.RegionCode,
                    DifficultyName = walkDto.Difficulty,
                },
                transaction: tx,
                cancellationToken: ct
            );

            var result = await conn.QuerySingleOrDefaultAsync<WalkRow>(cmd);
            return result?.ToWalkModel();
        };
    }
}
