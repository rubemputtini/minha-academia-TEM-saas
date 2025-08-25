using Microsoft.Extensions.DependencyInjection;
using MinhaAcademiaTEM.Application.Caching;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.Services.Helpers;
using MinhaAcademiaTEM.Application.Services.Subscriptions;
using MinhaAcademiaTEM.Application.Subscriptions;

namespace MinhaAcademiaTEM.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<EntityLookup>();
        services.AddScoped<AccessChecks>();
        services.AddScoped<SlugGenerator>();
        services.AddScoped<IAppCacheService, AppCacheService>();

        services.AddSingleton<IPlanCapabilityResolver, PlanCapabilityResolver>();
        services.AddScoped<IPlanRulesService, PlanRulesService>();

        return services;
    }
}