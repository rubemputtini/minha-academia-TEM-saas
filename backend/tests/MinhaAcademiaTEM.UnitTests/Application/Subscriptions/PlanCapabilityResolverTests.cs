using FluentAssertions;
using MinhaAcademiaTEM.Application.Subscriptions;
using MinhaAcademiaTEM.Domain.Constants;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.UnitTests.Application.Subscriptions;

public class PlanCapabilityResolverTests
{
    private readonly PlanCapabilityResolver _resolver = new();

    [Theory]
    [InlineData(Capability.ModifyEquipmentStatus)]
    [InlineData(Capability.ManageUserEquipmentSelection)]
    [InlineData(Capability.ManageCustomEquipment)]
    [InlineData(Capability.UploadCustomEquipmentMedia)]
    public void Trial_Should_Not_Have_Known_Capabilities(Capability cap)
    {
        _resolver.HasCapability(SubscriptionPlan.Trial, cap).Should().BeFalse();
    }

    [Fact]
    public void Basic_Should_Have_ModifyEquipmentStatus()
    {
        _resolver.HasCapability(SubscriptionPlan.Basic, Capability.ModifyEquipmentStatus).Should().BeTrue();
    }

    [Theory]
    [InlineData(Capability.ManageUserEquipmentSelection)]
    [InlineData(Capability.ManageCustomEquipment)]
    [InlineData(Capability.UploadCustomEquipmentMedia)]
    public void Basic_Should_Not_Have_Premium_Capabilities(Capability cap)
    {
        _resolver.HasCapability(SubscriptionPlan.Basic, cap).Should().BeFalse();
    }

    [Theory]
    [InlineData(Capability.ModifyEquipmentStatus)]
    [InlineData(Capability.ManageUserEquipmentSelection)]
    [InlineData(Capability.ManageCustomEquipment)]
    [InlineData(Capability.UploadCustomEquipmentMedia)]
    public void Unlimited_Should_Have_All_Known_Capabilities(Capability cap)
    {
        _resolver.HasCapability(SubscriptionPlan.Unlimited, cap).Should().BeTrue();
    }

    [Fact]
    public void Unknown_Plan_Should_Return_False()
    {
        var unknown = (SubscriptionPlan)999;
        _resolver.HasCapability(unknown, Capability.ModifyEquipmentStatus).Should().BeFalse();
    }
}