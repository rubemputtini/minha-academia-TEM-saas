using FluentAssertions;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.UnitTests.Domain.Entities;

public class CoachTests
{
    private static Address Addr() =>
        new(
            street: "Rua X",
            number: "123",
            complement: null,
            neighborhood: "Centro",
            city: "Porto",
            state: "pt",
            country: "pt",
            postalCode: "4000-000",
            latitude: null,
            longitude: null,
            coachId: Guid.NewGuid()
        );

    private static Coach NewCoach(string name = "Rubem", string email = "rubem@test.com", string slug = "rubem") =>
        new(Guid.NewGuid(), name, email, slug, Addr());

    [Fact]
    public void Constructor_Should_Trim_Name_And_Email_And_Start_In_Trial()
    {
        var coach = NewCoach("  Rubem  ", "  rubem@test.com  ");

        coach.Name.Should().Be("Rubem");
        coach.Email.Should().Be("rubem@test.com");
        coach.SubscriptionStatus.Should().Be(SubscriptionStatus.Trial);
    }

    [Fact]
    public void UpdateName_Should_Trim_And_Change_Name()
    {
        var coach = NewCoach();
        coach.UpdateName("  Novo Nome  ");
        coach.Name.Should().Be("Novo Nome");
    }

    [Fact]
    public void UpdateEmail_Should_Trim()
    {
        var coach = NewCoach();
        coach.UpdateEmail("  novo@mail.com  ");
        coach.Email.Should().Be("novo@mail.com");
    }

    [Fact]
    public void SetStripeData_Should_Set_Customer_And_Subscription()
    {
        var coach = NewCoach();
        coach.SetStripeData("cus_123", "sub_456");

        coach.StripeCustomerId.Should().Be("cus_123");
        coach.StripeSubscriptionId.Should().Be("sub_456");
    }

    [Fact]
    public void SetCanceled_Should_Set_Status_And_Clear_StripeSubscriptionId()
    {
        var coach = NewCoach();
        coach.SetStripeData("cus_1", "sub_1");

        coach.SetCanceled();

        coach.SubscriptionStatus.Should().Be(SubscriptionStatus.Canceled);
        coach.StripeSubscriptionId.Should().BeNull();
        coach.HasAccess.Should().BeFalse();
        coach.HasValidSubscription.Should().BeFalse();
    }

    [Fact]
    public void CancelSubscriptionNow_Should_Set_Canceled_And_EndAt_Now()
    {
        var coach = NewCoach();

        coach.CancelSubscriptionNow();

        coach.SubscriptionStatus.Should().Be(SubscriptionStatus.Canceled);
        coach.SubscriptionEndAt.Should().NotBeNull();
        coach.SubscriptionEndAt.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ScheduleCancellationAt_Active_Status_Should_Mark_IsCancellationScheduled()
    {
        var coach = NewCoach();
        var future = DateTime.UtcNow.AddDays(7);

        coach.SetSubscription(SubscriptionPlan.Basic, SubscriptionStatus.Active, null);
        coach.ScheduleCancellationAt(future);

        coach.SubscriptionEndAt.Should().Be(future);
        coach.IsCancellationScheduled.Should().BeTrue();
    }

    [Fact]
    public void IsCancellationScheduled_Should_BeFalse_When_NotActive_Or_PastDate()
    {
        var coach = NewCoach();

        coach.SetTrial();
        coach.ScheduleCancellationAt(DateTime.UtcNow.AddDays(7));
        coach.IsCancellationScheduled.Should().BeFalse();

        coach.SetSubscription(SubscriptionPlan.Basic, SubscriptionStatus.Active, null);
        coach.ScheduleCancellationAt(DateTime.UtcNow.AddDays(-1));
        coach.IsCancellationScheduled.Should().BeFalse();
    }

    [Fact]
    public void UndoScheduledCancellation_Should_Clear_EndAt()
    {
        var coach = NewCoach();
        var future = DateTime.UtcNow.AddDays(3);
        coach.SetSubscription(SubscriptionPlan.Basic, SubscriptionStatus.Active, null);
        coach.ScheduleCancellationAt(future);

        coach.UndoScheduledCancellation();

        coach.SubscriptionEndAt.Should().BeNull();
        coach.IsCancellationScheduled.Should().BeFalse();
    }

    [Fact]
    public void Access_Flags_Should_Reflect_Status()
    {
        var coach = NewCoach();

        coach.SetTrial();
        coach.HasAccess.Should().BeTrue();
        coach.HasValidSubscription.Should().BeFalse();

        coach.SetSubscription(SubscriptionPlan.Basic, SubscriptionStatus.Active, DateTime.UtcNow.AddMonths(1));
        coach.HasAccess.Should().BeTrue();
        coach.HasValidSubscription.Should().BeTrue();

        coach.SetCanceled();
        coach.HasAccess.Should().BeFalse();
        coach.HasValidSubscription.Should().BeFalse();
    }

    [Fact]
    public void SetSlug_Should_Update_Slug()
    {
        var coach = NewCoach(slug: "old");
        coach.SetSlug("new");
        coach.Slug.Should().Be("new");
    }
}