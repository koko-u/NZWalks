using System;

namespace NZWalk.Infrastructure.Rows;

public sealed class WalkRow
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public required double LengthKm { get; set; }

    public string? ImageUrl { get; set; }

    public required Guid RegionId { get; set; }

    public required string RegionCode { get; set; }

    public required string RegionName { get; set; }

    public string? RegionImageUrl { get; set; }

    public required Guid DifficultyId { get; set; }

    public required string DifficultyName { get; set; }
}
