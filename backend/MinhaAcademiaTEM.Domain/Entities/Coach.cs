namespace MinhaAcademiaTEM.Domain.Entities;

public class Coach : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public Address Address { get; private set; } = null!;
    public bool IsActive { get; private set; } = false;

    public User? User { get; private set; }

    public SubscriptionStatus SubscriptionStatus { get; private set; } = SubscriptionStatus.Trial;
    public SubscriptionPlan SubscriptionPlan { get; private set; } = SubscriptionPlan.Trial;
    public DateTime? SubscriptionEndAt { get; private set; }

    public string? StripeCustomerId { get; private set; }
    public string? StripeSubscriptionId { get; private set; }

    private readonly List<Gym> _gyms = [];
    public IReadOnlyCollection<Gym> Gyms => _gyms.AsReadOnly();

    private readonly List<Equipment> _equipments = [];
    public IReadOnlyCollection<Equipment> Equipments => _equipments.AsReadOnly();

    protected Coach()
    {
    }

    public Coach(Guid id, string name, string email, string slug, Address address) : base(id)
    {
        Name = name.Trim();
        Email = email.Trim();
        Slug = slug;
        Address = address;
        IsActive = true;
        SubscriptionStatus = SubscriptionStatus.Trial;
        SubscriptionPlan = SubscriptionPlan.Trial;
    }

    public void UpdateName(string name)
    {
        Name = name.Trim();
        User?.UpdateName(name);
    }

    public void UpdateEmail(string email) => Email = email.Trim();

    public void Activate() => IsActive = true;
    public void DeActivate() => IsActive = false;

    public void SetSubscription(SubscriptionPlan subscriptionPlan, DateTime? subscriptionEndAt)
    {
        SubscriptionPlan = subscriptionPlan;
        SubscriptionEndAt = subscriptionEndAt;
        SubscriptionStatus = SubscriptionStatus.Active;
    }

    public void SetTrial()
    {
        SubscriptionStatus = SubscriptionStatus.Trial;
        SubscriptionPlan = SubscriptionPlan.Trial;
        SubscriptionEndAt = null;
    }

    public void SetStripeData(string stripeCustomerId, string stripeSubscriptionId)
    {
        StripeCustomerId = stripeCustomerId;
        StripeSubscriptionId = stripeSubscriptionId;
    }

    public void SetSlug(string slug) => Slug = slug;

    public void SetUser(User user) => User = user;
}