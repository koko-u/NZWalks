using System;

namespace NZWalk.Infrastructure.Rows;

public sealed class UserRow
{
    public required Guid Id { get; set; }
    public string? DisplayName { get; set; }
    public required string Email { get; set; }
}
