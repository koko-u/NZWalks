using System;
using AutoRegisterAnnotation;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NZWalks.Core.Features.Auth.Models;
using NZWalks.Core.Features.Auth.Settings;

namespace NZWalks.Core;

public static class CoreServiceCollectionExtension
{
    public static IServiceCollection AddCoreServices(
        this IServiceCollection services,
        Action<ServiceTypePair>? onRegistered = null
    )
    {
        services.AddValidatorsFromAssemblyContaining(typeof(Core));
        services.AddAutoRegisterServices(typeof(Core), onRegistered);

        // パスワードハッシュ器
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        // JWT トークンの設定値
        services
            .AddOptions<JwtOptions>()
            .BindConfiguration("Jwt")
            .Validate(
                option => option.SigningKey.Length >= 32,
                "Signing key must be at least 32 characters long"
            )
            .Validate(
                option => option.AccessTokenExpirationMinutes is > 0 and <= 1440,
                "Access token expiration must be between 1 and 1440 minutes"
            )
            .Validate(
                option => option.RefreshTokenExpirationDays is > 0 and <= 365,
                "Refresh token expiration must be between 1 and 365 days"
            )
            .ValidateOnStart();

        return services;
    }
}
