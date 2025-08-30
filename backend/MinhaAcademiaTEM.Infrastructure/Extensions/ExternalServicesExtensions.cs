using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinhaAcademiaTEM.Application.Services.Billing;
using MinhaAcademiaTEM.Application.Services.Emails;
using MinhaAcademiaTEM.Application.Services.Subscriptions;
using MinhaAcademiaTEM.Application.Services.Webhooks;
using MinhaAcademiaTEM.Domain.Configuration;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Services;
using Stripe;

namespace MinhaAcademiaTEM.Infrastructure.Extensions;

public static class ExternalServicesExtensions
{
    public static void ConfigureExternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpConfiguration>(configuration.GetSection("Smtp"));
        services.Configure<StripeApiConfiguration>(configuration.GetSection("Stripe"));

        services.AddHttpContextAccessor();

        var secretKey = configuration["Stripe:SecretKey"];

        if (string.IsNullOrWhiteSpace(secretKey))
            throw new InvalidOperationException("Stripe SecretKey não configurada.");

        services.AddSingleton<IStripeClient>(_ => new StripeClient(new StripeClientOptions
        {
            ApiKey = secretKey
        }));

        services.AddSingleton(sp => new PromotionCodeService(sp.GetRequiredService<IStripeClient>()));
        services.AddSingleton(sp => new SubscriptionService(sp.GetRequiredService<IStripeClient>()));

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICheckoutSessionReader, StripeCheckoutSessionReader>();
        services.AddScoped<ICheckoutSessionsService, StripeCheckoutSessionsService>();
        services.AddScoped<IBillingPortalService, StripeBillingPortalService>();
        services.AddScoped<ISubscriptionAppService, SubscriptionAppService>();
        services.AddScoped<ISubscriptionSyncService, SubscriptionSyncService>();
        services.AddScoped<IStripeWebhookService, StripeWebhookService>();
        services.AddScoped<IAdminSubscriptionService, AdminSubscriptionService>();
        services.AddScoped<IStripeReferralService, StripeReferralService>();
        services.AddScoped<IReferralCreditsService, ReferralCreditsService>();
        services.AddScoped<IInvoiceDiscountReader, InvoiceDiscountReader>();
        services.AddScoped<ISubscriptionSummaryReader, StripeSubscriptionSummaryReader>();
    }
}