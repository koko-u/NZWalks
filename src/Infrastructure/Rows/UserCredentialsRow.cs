using System;

namespace NZWalk.Infrastructure.Rows;

public sealed class UserCredentialsRow
{
    public required Guid Id { get; set; }
    public string? DisplayName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
}
