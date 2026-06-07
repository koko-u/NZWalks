namespace NZWalks.Core.Features.Auth.Dto;

public sealed class RegisterUserDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? DisplayName { get; set; }
}
