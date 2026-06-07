namespace NZWalks.Core.Features.Auth.Result;

public abstract record class RefreshResult
{
    private RefreshResult() { }

    public sealed record class Success(string AccessToken, string RefreshToken) : RefreshResult;

    public sealed record class Invalid() : RefreshResult;

    public sealed record class Expired() : RefreshResult;
}
