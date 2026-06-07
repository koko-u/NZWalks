using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace NZWalk.Api.OpenApiTransformers;

public sealed class AuthOperationTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken ct
    )
    {
        try
        {
            var hasAnonymous = context
                .Description.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>()
                .Any();

            if (hasAnonymous)
            {
                return Task.CompletedTask;
            }

            var securityKey = new OpenApiSecuritySchemeReference(
                JwtBearerDefaults.AuthenticationScheme
            );
            var security = new OpenApiSecurityRequirement { { securityKey, [] } };

            operation.Security ??= [];
            operation.Security.Add(security);
            return Task.CompletedTask;
        }
        catch (Exception exception)
        {
            return Task.FromException(exception);
        }
    }
}
