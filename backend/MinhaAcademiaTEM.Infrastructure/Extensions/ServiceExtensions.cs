using Microsoft.Extensions.DependencyInjection;
using MinhaAcademiaTEM.Application.Services.Auth;

namespace MinhaAcademiaTEM.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        
        services.AddTransient<ITokenService, TokenService>();
    }
}