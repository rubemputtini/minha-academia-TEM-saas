using FluentAssertions;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;
using Moq;

namespace MinhaAcademiaTEM.UnitTests.Application.Common;

public class EntityLookupTests
{
    private readonly Mock<IUserRepository> _users = new();
    private readonly Mock<ICoachRepository> _coaches = new();
    private readonly Mock<IGymRepository> _gyms = new();

    private EntityLookup Lookup() => new(_users.Object, _coaches.Object, _gyms.Object);

    [Fact]
    public async Task GetUserAsync_Should_Return_User_When_Found()
    {
        var id = Guid.NewGuid();
        var user = TestData.User(id: id);

        _users.Setup(u => u.GetByIdAsync(id)).ReturnsAsync(user);

        var lookup = Lookup();
        var result = await lookup.GetUserAsync(id);

        result.Should().BeSameAs(user);
    }

    [Fact]
    public async Task GetUserAsync_Should_Throw_NotFound_When_Missing()
    {
        var id = Guid.NewGuid();

        _users.Setup(u => u.GetByIdAsync(id)).ReturnsAsync((User?)null);

        var lookup = Lookup();
        var result = () => lookup.GetUserAsync(id);

        await result.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetCoachAsync_Should_Return_Coach_When_Found()
    {
        var id = Guid.NewGuid();
        var coach = TestData.Coach(id);

        _coaches.Setup(c => c.GetByIdAsync(id)).ReturnsAsync(coach);

        var lookup = Lookup();
        var result = await lookup.GetCoachAsync(id);

        result.Should().BeSameAs(coach);
    }

    [Fact]
    public async Task GetCoachAsync_Should_Throw_NotFound_When_Missing()
    {
        var id = Guid.NewGuid();

        _coaches.Setup(c => c.GetByIdAsync(id)).ReturnsAsync((Coach?)null);

        var lookup = Lookup();
        var result = () => lookup.GetCoachAsync(id);

        await result.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetCoachByUserIdAsync_Should_Return_Coach_When_Found()
    {
        var userId = Guid.NewGuid();
        var coach = TestData.Coach(userId);

        _coaches.Setup(c => c.GetByUserIdAsync(userId)).ReturnsAsync(coach);

        var lookup = Lookup();
        var result = await lookup.GetCoachByUserIdAsync(userId);

        result.Should().BeSameAs(coach);
    }

    [Fact]
    public async Task GetCoachByUserIdAsync_Should_Throw_NotFound_When_Missing()
    {
        var userId = Guid.NewGuid();

        _coaches.Setup(c => c.GetByUserIdAsync(userId)).ReturnsAsync((Coach?)null);

        var lookup = Lookup();
        var result = () => lookup.GetCoachByUserIdAsync(userId);

        await result.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetCoachByStripeCustomerIdAsync_Should_Return_Coach_When_Found()
    {
        const string customerId = "cus_123";
        var coach = TestData.Coach();

        _coaches.Setup(c => c.GetByStripeCustomerIdAsync(customerId)).ReturnsAsync(coach);

        var lookup = Lookup();
        var result = await lookup.GetCoachByStripeCustomerIdAsync(customerId);

        result.Should().BeSameAs(coach);
    }

    [Fact]
    public async Task GetCoachByStripeCustomerIdAsync_Should_Throw_NotFound_When_Missing()
    {
        const string customerId = "cus_missing";

        _coaches.Setup(c => c.GetByStripeCustomerIdAsync(customerId)).ReturnsAsync((Coach?)null);

        var lookup = Lookup();
        var result = () => lookup.GetCoachByStripeCustomerIdAsync(customerId);

        await result.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetGymByUserIdAsync_Should_Return_Gym_When_Found()
    {
        var userId = Guid.NewGuid();
        var gym = TestData.Gym(userId: userId);

        _gyms.Setup(g => g.GetByUserIdAsync(userId)).ReturnsAsync(gym);

        var lookup = Lookup();
        var result = await lookup.GetGymByUserIdAsync(userId);

        result.Should().BeSameAs(gym);
    }

    [Fact]
    public async Task GetGymByUserIdAsync_Should_Throw_NotFound_When_Missing()
    {
        var userId = Guid.NewGuid();

        _gyms.Setup(g => g.GetByUserIdAsync(userId)).ReturnsAsync((Gym?)null);

        var lookup = Lookup();
        var result = () => lookup.GetGymByUserIdAsync(userId);

        await result.Should().ThrowAsync<NotFoundException>();
    }
}