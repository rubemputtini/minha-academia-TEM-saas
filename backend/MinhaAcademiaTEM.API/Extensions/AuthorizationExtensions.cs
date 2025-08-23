using Microsoft.AspNetCore.Authorization;
using MinhaAcademiaTEM.API.Security;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Infrastructure.Security;

namespace MinhaAcademiaTEM.API.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection ConfigureAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy("CoachHasAccess", policy =>
                policy.RequireAuthenticatedUser()
                    .RequireRole(nameof(UserRole.Coach), nameof(UserRole.Admin))
                    .AddRequirements(new CoachHasAccessRequirement()));

        services.AddScoped<IAuthorizationHandler, CoachHasAccessHandler>();
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, ApiAuthorizationResultHandler>();

        return services;
    }
}