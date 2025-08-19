using Microsoft.Extensions.Options;
using MinhaAcademiaTEM.Application.DTOs.Billing;
using MinhaAcademiaTEM.Application.DTOs.Subscriptions;
using MinhaAcademiaTEM.Application.Services.Billing;
using MinhaAcademiaTEM.Application.Services.Subscriptions;
using MinhaAcademiaTEM.Application.Services.Webhooks;
using MinhaAcademiaTEM.Domain.Configuration;
using Stripe;
using Stripe.Checkout;

namespace MinhaAcademiaTEM.Infrastructure.Services;

public class StripeWebhookService(
    IOptions<StripeApiConfiguration> stripeOptions,
    ICheckoutSessionsService checkoutService,
    ISubscriptionSyncService subscriptionService
) : IStripeWebhookService
{
    private readonly string _secret = stripeOptions.Value.WebhookSecret;

    public async Task HandleAsync(string payload, string? signature)
    {
        var stripeEvent = EventUtility.ConstructEvent(payload, signature, _secret);

        switch (stripeEvent.Type)
        {
            case EventTypes.CheckoutSessionCompleted:
                var session = (Session)stripeEvent.Data.Object;

                await checkoutService.ProcessCheckoutCompletedAsync(new CheckoutSessionCompletedRequest
                {
                    Mode = session.Mode,
                    ClientReferenceId = session.ClientReferenceId,
                    CustomerId = session.CustomerId,
                    SubscriptionId = session.SubscriptionId,
                    Metadata = session.Metadata,
                });

                break;

            case EventTypes.CustomerSubscriptionCreated:
            case EventTypes.CustomerSubscriptionUpdated:
            case EventTypes.CustomerSubscriptionDeleted:
                var subscription = (Subscription)stripeEvent.Data.Object;
                var request = MapUpdateRequest(subscription);
                await subscriptionService.UpdateAsync(request);

                break;
        }
    }

    private static UpdateSubscriptionRequest MapUpdateRequest(Subscription s)
    {
        return new UpdateSubscriptionRequest
        {
            CustomerId = s.CustomerId,
            SubscriptionId = s.Id,
            PriceId = s.Items.Data[0].Price.Id,
            StripeStatus = s.Status
        };
    }
}