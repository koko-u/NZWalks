using System;

namespace NZWalk.Infrastructure.Rows;

public sealed class DifficultyRow
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
}
