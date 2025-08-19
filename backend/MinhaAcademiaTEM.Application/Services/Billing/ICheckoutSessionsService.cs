using MinhaAcademiaTEM.Application.DTOs.Billing;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Application.Services.Billing;

public interface ICheckoutSessionsService
{
    Task<string> CreateSignupAsync(SubscriptionPlan subscriptionPlan);
    Task<string> CreateCoachSubscriptionAsync(SubscriptionPlan subscriptionPlan);
    Task ProcessCheckoutCompletedAsync(CheckoutSessionCompletedRequest request);
}