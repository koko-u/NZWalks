using System;

namespace NZWalk.Infrastructure.Rows;

public sealed class RegionRow
{
    public required Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }
}
