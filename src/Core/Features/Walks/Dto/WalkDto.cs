using System;
using NZWalks.Core.Features.Difficulties.Models;
using NZWalks.Core.Features.Regions.Models;

namespace NZWalks.Core.Features.Walks.Dto;

public record WalkDto(
    Guid Id,
    string Name,
    string? Description,
    double LengthKm,
    string? ImageUrl,
    Region Region,
    Difficulty Difficulty
);
