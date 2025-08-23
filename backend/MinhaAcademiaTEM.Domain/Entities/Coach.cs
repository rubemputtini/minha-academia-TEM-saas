namespace MinhaAcademiaTEM.Domain.Entities;

public class Coach : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public Address Address { get; private set; } = null!;

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
        Address = address;
        SetSlug(slug);
        SetTrial();
    }

    public void UpdateName(string name)
    {
        Name = name.Trim();
        User?.UpdateName(name);
    }

    public void UpdateEmail(string email) => Email = email.Trim();

    public void SetSubscription(
        SubscriptionPlan subscriptionPlan, SubscriptionStatus subscriptionStatus, DateTime? subscriptionEndAt)
    {
        SubscriptionPlan = subscriptionPlan;
        SubscriptionStatus = subscriptionStatus;
        SubscriptionEndAt = subscriptionEndAt;
    }

    public void ScheduleCancellationAt(DateTime endAtUtc)
    {
        SubscriptionEndAt = endAtUtc;
    }

    public void UndoScheduledCancellation()
    {
        SubscriptionEndAt = null;
    }

    public void CancelSubscriptionNow()
    {
        SetCanceled();
        SubscriptionEndAt = DateTime.UtcNow;
    }

    public void SetCanceled()
    {
        SubscriptionStatus = SubscriptionStatus.Canceled;
        StripeSubscriptionId = null;
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

    public bool HasValidSubscription => SubscriptionStatus == SubscriptionStatus.Active;

    public bool HasAccess =>
        SubscriptionStatus is SubscriptionStatus.Active or SubscriptionStatus.Trial;

    public bool IsCancellationScheduled => SubscriptionStatus == SubscriptionStatus.Active &&
                                           SubscriptionEndAt != null && SubscriptionEndAt > DateTime.UtcNow;
}