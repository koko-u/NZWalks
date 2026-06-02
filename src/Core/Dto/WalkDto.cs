using System;
using NZWalks.Core.Models;

namespace NZWalks.Core.Dto;

public record WalkDto(
    Guid Id,
    string Name,
    string? Description,
    double LengthKm,
    string? ImageUrl,
    Region Region,
    Difficulty Difficulty
);
