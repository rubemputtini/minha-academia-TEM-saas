using Microsoft.Extensions.DependencyInjection;
using MinhaAcademiaTEM.Application.Services.Account;
using MinhaAcademiaTEM.Application.Services.Admins;
using MinhaAcademiaTEM.Application.Services.Auth;
using MinhaAcademiaTEM.Application.Services.Coaches;
using MinhaAcademiaTEM.Application.Services.Equipments;
using MinhaAcademiaTEM.Application.Services.EquipmentSelections;

namespace MinhaAcademiaTEM.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ICoachService, CoachService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IBaseEquipmentService, BaseEquipmentService>();
        services.AddScoped<IEquipmentService, EquipmentService>();
        services.AddScoped<IEquipmentSelectionService, EquipmentSelectionService>();
        
        services.AddTransient<ITokenService, TokenService>();
    }
}