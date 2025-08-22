using MinhaAcademiaTEM.Application.DTOs.Billing;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Application.Services.Billing;

public interface ICheckoutSessionsService
{
    Task<string> CreateSignupAsync(SubscriptionPlan subscriptionPlan, string idempotencyKey);
    Task<string> CreateCoachSubscriptionAsync(SubscriptionPlan subscriptionPlan, string idempotencyKey);
    Task ProcessCheckoutCompletedAsync(CheckoutSessionCompletedRequest request);
}