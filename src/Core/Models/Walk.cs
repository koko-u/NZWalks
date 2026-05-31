using System;
using UnitsNet;

namespace NZWalks.Core.Models;

public sealed class Walk
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required Length Length { get; set; }
    public string? ImageUrl { get; set; }
    public required Region Region { get; set; }
    public required Difficulty Difficulty { get; set; }
}
