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

        services.AddSingleton<PromotionCodeService>();
        services.AddSingleton<SubscriptionService>();

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

        var secretKey = configuration["Stripe:SecretKey"];

        if (string.IsNullOrWhiteSpace(secretKey))
            throw new InvalidOperationException("Stripe SecretKey não configurada.");

        StripeConfiguration.ApiKey = secretKey;
    }
}