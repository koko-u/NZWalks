using System;

namespace NZWalks.Core.Features.Auth.Models;

public sealed class User
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public string? DisplayName { get; set; }
}
