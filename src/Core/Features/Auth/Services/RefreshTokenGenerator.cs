using System;
using System.Security.Cryptography;
using System.Text;
using AutoRegisterAnnotation;
using Microsoft.Extensions.DependencyInjection;

namespace NZWalks.Core.Features.Auth.Services;

[AutoRegisterService(Lifetime = ServiceLifetime.Singleton)]
public sealed class RefreshTokenGenerator
{
    public string Generate()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    public string Hash(string rawRefreshToken)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawRefreshToken));
        return Convert.ToBase64String(bytes);
    }
}
