namespace NZWalks.Core.Features.Auth.Settings;

public sealed class JwtOptions
{
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string SigningKey { get; set; }
    public required int AccessTokenExpirationMinutes { get; set; }
    public required int RefreshTokenExpirationDays { get; set; }
}
