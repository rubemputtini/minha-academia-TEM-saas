using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Infrastructure.Persistence;

namespace MinhaAcademiaTEM.Infrastructure.Extensions;

public static class IdentityExtensions
{
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
    }
}