using MinhaAcademiaTEM.Application.DTOs.Billing;
using MinhaAcademiaTEM.Application.Services.Billing;
using Stripe.Checkout;

namespace MinhaAcademiaTEM.Infrastructure.Services;

public class StripeCheckoutSessionReader : ICheckoutSessionReader
{
    public async Task<CoachPreFillResponse> GetPrefillAsync(string sessionId)
    {
        if (string.IsNullOrWhiteSpace(sessionId))
            throw new ArgumentException("O session_id é obrigatório.", nameof(sessionId));

        var service = new SessionService();
        var session = await service.GetAsync(sessionId);

        if (!string.Equals(session.Status, "complete", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("A sessão de checkout ainda não foi concluída.");

        if (!string.Equals(session.Mode, "subscription", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Sessão inválida para o fluxo de assinatura.");

        if (string.IsNullOrWhiteSpace(session.CustomerId))
            throw new InvalidOperationException("Cliente não vinculado à sessão do Stripe.");

        var details = session.CustomerDetails;
        var address = details?.Address;

        var response = new CoachPreFillResponse
        {
            Name = details?.Name ?? string.Empty,
            Email = details?.Email ?? string.Empty,
            PhoneNumber = details?.Phone ?? string.Empty,
            Address = new AddressPrefillResponse
            {
                Street = address?.Line1 ?? string.Empty,
                Complement = address?.Line2 ?? string.Empty,
                City = address?.City ?? string.Empty,
                State = address?.State ?? string.Empty,
                Country = address?.Country ?? string.Empty,
                PostalCode = address?.PostalCode ?? string.Empty,
                Number = null,
                Neighborhood = null
            },
            SubscriptionPlan = session.Metadata != null && session.Metadata.TryGetValue("app_plan", out var plan)
                ? plan
                : string.Empty,
            StripeCustomerId = session.CustomerId,
            StripeSubscriptionId = session.SubscriptionId,
            ClientReferenceId = session.ClientReferenceId
        };

        return response;
    }
}