using System;

namespace NZWalks.Core.Features.Auth.Result;

public abstract record class AuthResult
{
    private AuthResult() { }

    public sealed record class Authenticated(Guid UserId, string AccessToken, string RefreshToken)
        : AuthResult;

    public sealed record class InvalidCredentials : AuthResult;
}
