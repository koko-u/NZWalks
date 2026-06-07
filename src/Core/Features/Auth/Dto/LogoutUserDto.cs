namespace NZWalks.Core.Features.Auth.Dto;

public sealed class LogoutUserDto
{
    public required string RefreshToken { get; init; }
}
