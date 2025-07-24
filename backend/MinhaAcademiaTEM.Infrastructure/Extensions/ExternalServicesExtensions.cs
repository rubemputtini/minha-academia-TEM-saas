using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinhaAcademiaTEM.Domain.Configuration;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Services;

namespace MinhaAcademiaTEM.Infrastructure.Extensions;

public static class ExternalServicesExtensions
{
    public static void ConfigureExternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpConfiguration>(configuration.GetSection("Smtp"));

        services.AddScoped<IEmailService, EmailService>();
    }
}