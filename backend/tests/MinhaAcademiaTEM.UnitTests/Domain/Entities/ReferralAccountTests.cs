using FluentAssertions;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.UnitTests.Domain.Entities;

public class ReferralAccountTests
{
    private static ReferralAccount NewReferralAccount(Guid coachId = default) => new(coachId);

    [Fact]
    public void Constructor_Should_Set_CoachId_And_Initialize_Defaults()
    {
        var coachId = Guid.NewGuid();
        var account = NewReferralAccount(coachId);

        account.CoachId.Should().Be(coachId);
        account.CreditsAvailable.Should().Be(0);
        account.LastAppliedPeriod.Should().BeNull();
        account.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void AddCredit_Should_Increment_Credits_And_Update_Timestamp()
    {
        var account = NewReferralAccount();

        account.AddCredit(1);

        account.CreditsAvailable.Should().Be(1);
        account.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void CanApplyForPeriod_Should_Return_False_When_No_Credits()
    {
        var account = NewReferralAccount();

        account.CanApplyForPeriod(DateTime.UtcNow).Should().BeFalse();
    }

    [Fact]
    public void CanApplyForPeriod_Should_Return_True_When_Credits_Available_And_Not_Applied_This_Period()
    {
        var account = NewReferralAccount();

        account.AddCredit(1);
        account.CanApplyForPeriod(DateTime.UtcNow).Should().BeTrue();
    }

    [Fact]
    public void MarkApplied_Should_Decrement_Credits_And_Set_LastAppliedPeriod_And_Update_Timestamp()
    {
        var account = NewReferralAccount();
        account.AddCredit(2);
        var period = new DateTime(2025, 08, 01);

        account.MarkApplied(period);

        account.CreditsAvailable.Should().Be(1);
        account.LastAppliedPeriod.Should().Be(202508);
        account.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void MarkApplied_Should_Not_Decrement_Below_Zero()
    {
        var account = NewReferralAccount();
        var period = new DateTime(2025, 08, 01);

        account.AddCredit(1);
        account.MarkApplied(period);

        account.CreditsAvailable.Should().Be(0);
        account.LastAppliedPeriod.Should().Be(202508);
    }

    [Fact]
    public void CanApplyForPeriod_Should_Return_False_When_Already_Applied_Same_Period()
    {
        var account = NewReferralAccount();
        var period = new DateTime(2025, 08, 01);
        account.AddCredit(1);
        account.MarkApplied(period);

        account.CanApplyForPeriod(period).Should().BeFalse();
    }

    [Fact]
    public void AddCredit_Then_MarkApplied_Should_Allow_Next_Month_But_Block_Current()
    {
        var account = NewReferralAccount();
        var current = new DateTime(2025, 08, 01);
        var next = current.AddMonths(1);

        account.AddCredit(2);

        account.CanApplyForPeriod(current).Should().BeTrue();
        account.MarkApplied(current);
        account.CanApplyForPeriod(current).Should().BeFalse();

        account.CanApplyForPeriod(next).Should().BeTrue();
        account.MarkApplied(next);
        account.CanApplyForPeriod(next).Should().BeFalse();
        account.CreditsAvailable.Should().Be(0);
    }
}