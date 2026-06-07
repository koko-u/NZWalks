namespace NZWalks.Core.Features.Walks.Dto;

public sealed class NewWalkDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double? LengthKm { get; set; }
    public string? ImageUrl { get; set; }
    public string? RegionCode { get; set; }
    public string? Difficulty { get; set; }
}
