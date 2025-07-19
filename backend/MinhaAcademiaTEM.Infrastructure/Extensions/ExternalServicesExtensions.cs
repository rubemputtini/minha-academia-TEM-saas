using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MinhaAcademiaTEM.Infrastructure.Extensions;

public static class ExternalServicesExtensions
{
    public static void ConfigureExternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        //TODO: services.Configure<SmtpConfiguration>(configuration.GetSection("Smtp"));
    }
}