namespace MinhaAcademiaTEM.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureRateLimiting();
        services.ConfigureAuthentication(configuration);
        services.ConfigureCors(configuration);

        services.AddMemoryCache();

        services.AddControllers();

        return services;
    }
}