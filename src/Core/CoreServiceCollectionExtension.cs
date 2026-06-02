using System;
using AutoRegisterAnnotation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

        return services;
    }
}
