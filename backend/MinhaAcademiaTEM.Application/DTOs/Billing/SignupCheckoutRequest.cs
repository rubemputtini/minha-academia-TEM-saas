using System.ComponentModel.DataAnnotations;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Application.DTOs.Billing;

public sealed class SignupCheckoutRequest
{
    [Required(ErrorMessage = "O novo plano da assinatura é obrigatório.")]
    public SubscriptionPlan SubscriptionPlan { get; init; }
}