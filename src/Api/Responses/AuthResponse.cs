using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace NZWalk.Api.Responses;

public sealed class AuthResponse
{
    public required string AccessToken { get; init; }

    public string TokenType { get; set; } = JwtBearerDefaults.AuthenticationScheme;

    public required int ExpiresIn { get; init; }

    public required string RefreshToken { get; init; }
}
