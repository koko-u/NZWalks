using NZWalks.Core.Models;
using Riok.Mapperly.Abstractions;
using UnitsNet;

namespace NZWalk.Api.Dto;

[Mapper(ThrowOnPropertyMappingNullMismatch = true)]
public static partial class WalkDtoMapper
{
    [MapProperty(nameof(Walk.Id), nameof(WalkDto.Id))]
    [MapProperty(nameof(Walk.Name), nameof(WalkDto.Name))]
    [MapProperty(nameof(Walk.Description), nameof(WalkDto.Description))]
    [MapProperty(nameof(Walk.Length), nameof(WalkDto.LengthKm), Use = nameof(MapLength))]
    [MapProperty(nameof(Walk.ImageUrl), nameof(WalkDto.ImageUrl))]
    [MapProperty(nameof(Walk.Region), nameof(WalkDto.Region))]
    [MapProperty(nameof(Walk.Difficulty), nameof(WalkDto.Difficulty))]
    public static partial WalkDto MapToDto(this Walk walk);

    private static double MapLength(Length length)
    {
        return length.Kilometers;
    }
}
