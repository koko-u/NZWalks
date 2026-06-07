using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using FluentValidation;
using MicroElements.AspNetCore.OpenApi.FluentValidation;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NZWalk.Api.Extensions;
using NZWalk.Api.OpenApiTransformers;
using NZWalk.Infrastructure;
using NZWalks.Core;
using NZWalks.Core.Features.Auth.Settings;
using Scalar.AspNetCore;
using Serilog;

// Bootstrap Logger
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

// Setup Infrastructure
Infrastructure.Setup();

// FluentValidation culture settings
ValidatorOptions.Global.LanguageManager.Culture = CultureInfo.InvariantCulture;

try
{
    var builder = WebApplication.CreateBuilder(args);
    // Configure Serilog
    builder.Host.UseSerilog(
        (context, provider, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(provider)
                .Enrich.FromLogContext();
        }
    );
    // Configure DI Validation
    builder.Host.UseDefaultServiceProvider(opts =>
    {
        opts.ValidateScopes = true;
        opts.ValidateOnBuild = true;
    });

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddProblemDetails();
    builder.Services.AddValidatorsFromAssemblyContaining<Program>();
    builder.Services.AddFluentValidationRulesToOpenApi();

    // Authentication & Authorization
    builder.Services.AddJwtBearerAuthentication(builder.Configuration);
    builder.Services.AddAuthorization();

    var logging = (ServiceTypePair srvPair) =>
    {
        var (serviceType, implementationType, lifetime) = srvPair;
        Log.Logger.Information(
            "Registering service: {implName} (implements {interfaceName}) with lifetime: {lifetime}",
            implementationType.Name,
            serviceType.Name,
            lifetime
        );
    };
    builder.Services.AddAutoRegisterServices<Program>(onRegistered: logging);
    builder.Services.AddInfrastructure(builder.Configuration, onRegistered: logging);
    builder.Services.AddCoreServices(onRegistered: logging);

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi(opts =>
    {
        opts.AddOperationTransformer(
            (operation, _, _) =>
            {
                operation.Summary = null;
                operation.Description = null;
                return Task.CompletedTask;
            }
        );
        opts.AddFluentValidationRules();
        opts.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        opts.AddOperationTransformer<AuthOperationTransformer>();
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(opts =>
        {
            opts.EnableDarkMode()
                .WithTitle("WatchStore Api Reference")
                .WithTheme(ScalarTheme.BluePlanet)
                .ShowOperationId()
                .WithDefaultHttpClient(ScalarTarget.Shell, ScalarClient.Curl)
                .WithDocumentDownloadType(DocumentDownloadType.Json)
                .WithJsonDocumentDownload()
                .PreserveSchemaPropertyOrder();
        });
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler();
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseSerilogRequestLogging();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Terminate application unexpectedly.");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
