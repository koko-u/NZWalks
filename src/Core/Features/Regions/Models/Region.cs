using System;

namespace NZWalks.Core.Features.Regions.Models;

public readonly record struct Region(Guid Id, string Code, string Name, string? ImageUrl);
