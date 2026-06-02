using System;
using AutoRegisterAnnotation;
using KozLibraries.DapperSqlHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NZWalk.Infrastructure.Settings;

namespace NZWalk.Infrastructure;

public static class InfrastructureServiceCollectionExtension
{
    /// <summary>
    /// Configure Infrastructure services
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="onRegistered"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<ServiceTypePair>? onRegistered = null
    )
    {
        // Get Database settings
        var databaseSetting =
            configuration.GetRequiredSection("Database").Get<DatabaseSetting>()
            ?? throw new InvalidOperationException("Database settings are required");

        services.AddAutoRegisterServices(typeof(Infrastructure), onRegistered);
        services.AddNpgsqlDataSource(databaseSetting.ConnectionString);
        services.AddSqlResource(opts =>
        {
            opts.SqlBasePath = "sql";
            opts.Assembly = typeof(Infrastructure).Assembly;
        });

        return services;
    }
}
