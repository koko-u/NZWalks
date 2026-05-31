using System;
using NZWalks.Core.Models;

namespace NZWalk.Api.Dto;

public record WalkDto(
    Guid Id,
    string Name,
    string? Description,
    double LengthKm,
    string? ImageUrl,
    Region Region,
    Difficulty Difficulty
);
