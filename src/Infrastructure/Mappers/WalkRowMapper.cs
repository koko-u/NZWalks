using System;
using NZWalk.Infrastructure.Rows;
using NZWalks.Core.Features.Difficulties.Models;
using NZWalks.Core.Features.Regions.Models;
using NZWalks.Core.Features.Walks.Models;
using Riok.Mapperly.Abstractions;
using UnitsNet;

namespace NZWalk.Infrastructure.Mappers;

[Mapper(ThrowOnPropertyMappingNullMismatch = true)]
public static partial class WalkRowMapper
{
    [MapProperty(nameof(WalkRow.LengthKm), nameof(Walk.Length), Use = nameof(MapLength))]
    [MapValue(nameof(Walk.Region), Use = nameof(EmptyRegion))]
    [MapValue(nameof(Walk.Difficulty), Use = nameof(EmptyDifficulty))]
    [MapperIgnoreSource(nameof(WalkRow.RegionId))]
    [MapperIgnoreSource(nameof(WalkRow.RegionCode))]
    [MapperIgnoreSource(nameof(WalkRow.RegionName))]
    [MapperIgnoreSource(nameof(WalkRow.RegionImageUrl))]
    [MapperIgnoreSource(nameof(WalkRow.DifficultyId))]
    [MapperIgnoreSource(nameof(WalkRow.DifficultyName))]
    public static partial Walk ToWalkModel(this WalkRow walkRow);

    [MapProperty(nameof(WalkRow.LengthKm), nameof(Walk.Length), Use = nameof(MapLength))]
    [MapperIgnoreSource(nameof(WalkRow.RegionId))]
    [MapperIgnoreSource(nameof(WalkRow.RegionCode))]
    [MapperIgnoreSource(nameof(WalkRow.RegionName))]
    [MapperIgnoreSource(nameof(WalkRow.RegionImageUrl))]
    [MapperIgnoreSource(nameof(WalkRow.DifficultyId))]
    [MapperIgnoreSource(nameof(WalkRow.DifficultyName))]
    public static partial Walk ToWalkModel(
        this WalkRow walkRow,
        Region region,
        Difficulty difficulty
    );

    private static Length MapLength(double lengthKm) => Length.FromKilometers(lengthKm);

    private static Region EmptyRegion() =>
        new Region
        {
            Id = Guid.Empty,
            Name = "Unknown",
            Code = "UNK",
            ImageUrl = "",
        };

    private static Difficulty EmptyDifficulty() =>
        new Difficulty { Id = Guid.Empty, Name = "Unknown" };
}
