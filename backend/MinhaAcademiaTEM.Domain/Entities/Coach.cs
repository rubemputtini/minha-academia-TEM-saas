namespace MinhaAcademiaTEM.Domain.Entities;

public class Coach : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public Address Address { get; set; } = null!;
    public bool IsActive { get; set; } = false;

    public SubscriptionStatus SubscriptionStatus { get; set; } = SubscriptionStatus.Trial;
    public SubscriptionPlan SubscriptionPlan { get; set; } = SubscriptionPlan.Trial;
    public DateTime? SubscriptionEndAt { get; set; }

    public string? StripeCustomerId { get; set; }
    public string? StripeSubscriptionId { get; set; }

    public ICollection<Gym> Gyms { get; set; } = new List<Gym>();
    public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
}