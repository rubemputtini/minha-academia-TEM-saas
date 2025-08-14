using System.ComponentModel.DataAnnotations;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Application.DTOs.Admin;

public sealed class UpdateCoachSubscriptionRequest
{
    [Required(ErrorMessage = "O novo plano da assinatura é obrigatório.")]
    public SubscriptionPlan SubscriptionPlan { get; init; }

    public DateTime? SubscriptionEndAt { get; init; }
}