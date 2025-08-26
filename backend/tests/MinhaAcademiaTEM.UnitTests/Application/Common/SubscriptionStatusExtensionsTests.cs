using FluentAssertions;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.UnitTests.Application.Common;

public class SubscriptionStatusExtensionsTests
{
    [Theory]
    [InlineData(SubscriptionStatus.Trial, "Teste")]
    [InlineData(SubscriptionStatus.Active, "Ativa")]
    [InlineData(SubscriptionStatus.PastDue, "Atrasada")]
    [InlineData(SubscriptionStatus.Canceled, "Cancelada")]
    public void ToDisplay_Should_Map_Known_Statuses(SubscriptionStatus status, string expected)
    {
        status.ToDisplay().Should().Be(expected);
    }

    [Fact]
    public void ToDisplay_Should_Fallback_To_ToString_For_Unknown_Value()
    {
        const SubscriptionStatus unknown = (SubscriptionStatus)999;
        unknown.ToDisplay().Should().Be("999");
    }
}