using System;

namespace NZWalks.Core.Features.Auth.Models;

public sealed class RefreshToken
{
    public required Guid Id { get; set; }

    public required Guid UserId { get; set; }

    public required string TokenHash { get; set; }

    public DateTime? ExpiresAt { get; set; }
}
