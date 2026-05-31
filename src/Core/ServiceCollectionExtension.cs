using System;
using AutoRegisterAnnotation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NZWalks.Core;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCoreServices(
        this IServiceCollection services,
        Action<ServiceTypePair>? onRegistered = null
    )
    {
        services.AddAutoRegisterServices(typeof(Core), onRegistered);

        return services;
    }
}
