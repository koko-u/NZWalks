using System;

namespace NZWalk.Infrastructure.Rows;

public sealed class RefreshTokenRow
{
    public required Guid Id { get; set; }

    public required Guid UserId { get; set; }

    public required string TokenHash { get; set; }

    public DateTime? ExpiresAt { get; set; }
}
