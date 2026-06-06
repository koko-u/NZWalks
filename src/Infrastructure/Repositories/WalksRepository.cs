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
using NZWalks.Core.Data;
using NZWalks.Core.Dto;
using NZWalks.Core.Extensions;
using NZWalks.Core.Models;
using NZWalks.Core.QueryParameters;
using NZWalks.Core.Repositories;
using NZWalks.Core.Tx;

namespace NZWalk.Infrastructure.Repositories;

[AutoRegisterService]
public sealed class WalksRepository(SqlResource sql) : IWalksRepository
{
    public Func<DbSession, CancellationToken, Task<IEnumerable<Walk>>> GetWalksAsync(
        WalkFilter filter,
        WalkOrder order,
        Paging paging
    )
    {
        return async (session, ct) =>
        {
            var template = await CreateSelectAllTemplate(filter, order, paging, ct);

            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                commandText: template.RawSql,
                parameters: template.Parameters,
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

    private async Task<SqlBuilder.Template> CreateSelectAllTemplate(
        WalkFilter filter,
        WalkOrder order,
        Paging paging,
        CancellationToken ct
    )
    {
        var builder = new SqlBuilder();
        var template = builder.AddTemplate(
            await sql.GetAsync("walks/select_all_template.sql", ct),
            new { Limit = paging.Limit(), Offset = paging.Offset() }
        );
        if (!string.IsNullOrEmpty(filter.NameLike))
        {
            builder.Where(
                """
                "W"."name" ILIKE @NameLike
                """,
                new { NameLike = $"%{filter.NameLike}%" }
            );
        }

        if (filter.MinLength.HasValue)
        {
            builder.Where(
                """
                "W"."length_km" >= @MinLength
                """,
                new { filter.MinLength }
            );
        }

        if (filter.MaxLength.HasValue)
        {
            builder.Where(
                """
                "W"."length_km" <= @MaxLength
                """,
                new { filter.MaxLength }
            );
        }

        if (!string.IsNullOrEmpty(filter.RegionCode))
        {
            builder.Where(
                """
                "R"."code" = @RegionCode
                """,
                new { filter.RegionCode }
            );
        }

        if (!string.IsNullOrEmpty(filter.Difficulty))
        {
            builder.Where(
                """
                "D"."name" = @Difficulty
                """,
                new { filter.Difficulty }
            );
        }

        foreach (var (key, direction) in order.OrderByFields())
        {
            key.When(OrderKey.Id)
                .Then(() =>
                {
                    builder.OrderBy(
                        $"""
                        "W"."id" {direction.Value}
                        """
                    );
                })
                .When(OrderKey.WalkName)
                .Then(() =>
                {
                    builder.OrderBy(
                        $"""
                        "W"."name" {direction.Value}
                        """
                    );
                })
                .When(OrderKey.Length)
                .Then(() =>
                {
                    builder.OrderBy(
                        $"""
                        "W"."length_km" {direction.Value}
                        """
                    );
                })
                .When(OrderKey.RegionCode)
                .Then(() =>
                {
                    builder.OrderBy(
                        $"""
                        "R"."code" {direction.Value}
                        """
                    );
                })
                .When(OrderKey.Difficulty)
                .Then(() =>
                {
                    builder.OrderBy(
                        $"""
                        "D"."name" {direction.Value}
                        """
                    );
                });
        }

        return template;
    }

    public Func<DbSession, CancellationToken, Task<int>> GetWalksTotalCount(WalkFilter filter)
    {
        return async (session, ct) =>
        {
            var template = await CreateSelectCountTemplate(filter, ct);
            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                commandText: template.RawSql,
                parameters: template.Parameters,
                transaction: tx,
                cancellationToken: ct
            );
            return await conn.ExecuteScalarAsync<int>(cmd);
        };
    }

    private async Task<SqlBuilder.Template> CreateSelectCountTemplate(
        WalkFilter filter,
        CancellationToken ct
    )
    {
        var builder = new SqlBuilder();
        var template = builder.AddTemplate(
            await sql.GetAsync("walks/select_count_template.sql", ct)
        );
        if (!string.IsNullOrEmpty(filter.NameLike))
        {
            builder.Where(
                """
                "W"."name" ILIKE @NameLike
                """,
                new { NameLike = $"%{filter.NameLike}%" }
            );
        }

        if (filter.MinLength.HasValue)
        {
            builder.Where(
                """
                "W"."length_km" >= @MinLength
                """,
                new { filter.MinLength }
            );
        }

        if (filter.MaxLength.HasValue)
        {
            builder.Where(
                """
                "W"."length_km" <= @MaxLength
                """,
                new { filter.MaxLength }
            );
        }

        if (!string.IsNullOrEmpty(filter.RegionCode))
        {
            builder.Where(
                """
                "R"."code" = @RegionCode
                """,
                new { filter.RegionCode }
            );
        }

        if (!string.IsNullOrEmpty(filter.Difficulty))
        {
            builder.Where(
                """
                "D"."name" = @Difficulty
                """,
                new { filter.Difficulty }
            );
        }

        return template;
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

    public Func<DbSession, CancellationToken, Task<Walk?>> DeleteWalkAsync(Guid id)
    {
        return async (session, ct) =>
        {
            var (conn, tx) = session;
            var cmd = new CommandDefinition(
                commandText: await sql.GetAsync("walks/delete_by_id.sql", ct),
                parameters: new { Id = id },
                transaction: tx,
                cancellationToken: ct
            );

            var result = await conn.QuerySingleOrDefaultAsync<WalkRow>(cmd);
            return result?.ToWalkModel();
        };
    }
}
