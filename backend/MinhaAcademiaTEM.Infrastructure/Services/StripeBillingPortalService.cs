using Microsoft.Extensions.Configuration;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.Services.Billing;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Infrastructure.Services;

public class StripeBillingPortalService(
    EntityLookup lookup,
    ICurrentUserService currentUser,
    IConfiguration configuration) : IBillingPortalService
{
    public async Task<string> CreateCustomerPortalSessionAsync()
    {
        var coach = await lookup.GetCoachByUserIdAsync(currentUser.GetUserId());

        if (string.IsNullOrWhiteSpace(coach.StripeCustomerId))
            throw new InvalidOperationException("Não existe uma conta stripe vinculada a esse treinador.");

        var returnUrl = $"{configuration["AppSettings:FrontendUrl"]}/conta/assinatura";

        var options = new Stripe.BillingPortal.SessionCreateOptions
        {
            Customer = coach.StripeCustomerId,
            ReturnUrl = returnUrl
        };

        var service = new Stripe.BillingPortal.SessionService();
        var session = await service.CreateAsync(options);

        if (string.IsNullOrWhiteSpace(session.Url))
            throw new InvalidOperationException("Stripe não retornou a URL do Billing Portal.");

        return session.Url;
    }
}