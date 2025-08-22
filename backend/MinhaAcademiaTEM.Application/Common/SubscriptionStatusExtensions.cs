using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Application.Common;

public static class SubscriptionStatusExtensions
{
    public static string ToDisplay(this SubscriptionStatus status) =>
        status switch
        {
            SubscriptionStatus.Trial => "Teste",
            SubscriptionStatus.Active => "Ativa",
            SubscriptionStatus.PastDue => "Atrasada",
            SubscriptionStatus.Canceled => "Cancelada",
            _ => status.ToString()
        };
}