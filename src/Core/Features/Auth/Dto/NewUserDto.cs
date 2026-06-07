namespace NZWalks.Core.Features.Auth.Dto;

public sealed class NewUserDto
{
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string? DisplayName { get; set; }
}
