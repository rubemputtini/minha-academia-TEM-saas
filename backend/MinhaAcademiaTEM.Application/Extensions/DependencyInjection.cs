using Microsoft.Extensions.DependencyInjection;
using MinhaAcademiaTEM.Application.Caching;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.Services;

namespace MinhaAcademiaTEM.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<EntityLookup>();
        services.AddScoped<AccessChecks>();
        services.AddScoped<SlugGenerator>();
        services.AddScoped<IAppCacheService, AppCacheService>();

        return services;
    }
}