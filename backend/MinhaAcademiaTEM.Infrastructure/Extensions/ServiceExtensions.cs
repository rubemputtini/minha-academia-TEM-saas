using Microsoft.Extensions.DependencyInjection;
using MinhaAcademiaTEM.Application.Services.Admins;
using MinhaAcademiaTEM.Application.Services.Auth;
using MinhaAcademiaTEM.Application.Services.Coaches;

namespace MinhaAcademiaTEM.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICoachService, CoachService>();
        services.AddScoped<IAdminService, AdminService>();
        
        services.AddTransient<ITokenService, TokenService>();
    }
}