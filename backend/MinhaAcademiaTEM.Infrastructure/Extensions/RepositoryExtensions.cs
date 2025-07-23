using Microsoft.Extensions.DependencyInjection;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Repositories;

namespace MinhaAcademiaTEM.Infrastructure.Extensions;

public static class RepositoryExtensions
{
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICoachRepository, CoachRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}