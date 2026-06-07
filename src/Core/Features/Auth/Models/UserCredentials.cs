using System;

namespace NZWalks.Core.Features.Auth.Models;

public sealed class UserCredentials
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string? DisplayName { get; set; }
}
