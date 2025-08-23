namespace MinhaAcademiaTEM.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureRateLimiting();
        services.ConfigureAuthentication(configuration);
        services.ConfigureAuthorizationPolicies();
        services.ConfigureCors(configuration);
        services.ConfigureCompression();
        services.AddMemoryCache();
        services.AddControllers();
        services.AddSwaggerGen();
        services.AddHealthChecks();

        return services;
    }
}