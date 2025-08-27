using FluentAssertions;
using MinhaAcademiaTEM.Application.Caching;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.Admin;
using MinhaAcademiaTEM.Application.DTOs.Coaches;
using MinhaAcademiaTEM.Application.DTOs.Users;
using MinhaAcademiaTEM.Application.Services.Admins;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;
using Moq;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.Admins;

public class AdminServiceTests
{
    private readonly Mock<IUserRepository> _users = new();
    private readonly Mock<ICoachRepository> _coaches = new();
    private readonly Mock<IGymRepository> _gyms = new();
    private readonly Mock<IAppCacheService> _cache = new();

    private AdminService Service() => new(
        new EntityLookup(_users.Object, _coaches.Object, _gyms.Object),
        _coaches.Object,
        _users.Object,
        _cache.Object
    );

    [Fact]
    public async Task GetAllCoachesAsync_Should_Return_From_Cache_When_Hit()
    {
        var page = 1;
        var pageSize = 10;
        string? search = null;
        _coaches.Setup(r => r.CountAsync(search)).ReturnsAsync(123);

        var cached = (Coaches: (IEnumerable<CoachResponse>)
        [
            new CoachResponse { Id = Guid.NewGuid(), Name = "Cached", Email = "c@t.com" }
        ], TotalCoaches: 123);

        _cache.Setup(c => c.TryGetValue<(IEnumerable<CoachResponse>, int)>(It.IsAny<string>(), out cached))
            .Returns(true);

        var service = Service();

        var (coaches, total) = await service.GetAllCoachesAsync(page, pageSize, search);

        total.Should().Be(123);
        coaches.Should().ContainSingle(c => c.Name == "Cached");

        _coaches.Verify(r => r.SearchAsync(It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        _users.Verify(r => r.GetClientsCountForCoachesAsync(It.IsAny<List<Guid>>()), Times.Never);
        _cache.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<(IEnumerable<CoachResponse>, int)>()), Times.Never);
    }

    [Fact]
    public async Task GetAllCoachesAsync_Should_Query_And_Cache_When_Miss()
    {
        var page = 1;
        var pageSize = 10;
        string? search = null;
        _coaches.Setup(r => r.CountAsync(search)).ReturnsAsync(1);

        var anyOut = default((IEnumerable<CoachResponse>, int));
        _cache.Setup(c => c.TryGetValue(It.IsAny<string>(), out anyOut))
            .Returns(false);

        var coach = TestData.Coach(name: "C1");
        var coachUser = TestData.User(name: "C1", email: "c1@test.com");
        coachUser.PhoneNumber = "999";
        coach.SetUser(coachUser);

        _coaches.Setup(r => r.SearchAsync(search, 0, pageSize))
            .ReturnsAsync(new List<Coach> { coach });
        _users.Setup(r => r.GetClientsCountForCoachesAsync(It.IsAny<List<Guid>>()))
            .ReturnsAsync(new Dictionary<Guid, int> { [coach.Id] = 5 });

        var service = Service();

        var (coaches, total) = await service.GetAllCoachesAsync(page, pageSize, search);

        total.Should().Be(1);
        var item = coaches.Single();
        item.Name.Should().Be("C1");
        item.PhoneNumber.Should().Be("999");
        item.ClientsCount.Should().Be(5);

        _cache.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<(IEnumerable<CoachResponse>, int)>()), Times.Once);
    }

    [Fact]
    public async Task GetAllUsersAsync_Should_Return_From_Cache_When_Hit()
    {
        var page = 1;
        var pageSize = 10;
        string? search = null;
        _users.Setup(r => r.CountAsync(search)).ReturnsAsync(10);

        var cached = (Users: (IEnumerable<UserResponse>)new[]
        {
            new UserResponse { Id = Guid.NewGuid(), Name = "U1", Email = "u1@t.com" }
        }, TotalUsers: 10);

        _cache.Setup(c => c.TryGetValue<(IEnumerable<UserResponse>, int)>(It.IsAny<string>(), out cached))
            .Returns(true);

        var service = Service();

        var (users, total) = await service.GetAllUsersAsync(page, pageSize, search);

        total.Should().Be(10);
        users.Should().ContainSingle(u => u.Name == "U1");

        _users.Verify(r => r.SearchAsync(It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        _cache.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<(IEnumerable<UserResponse>, int)>()), Times.Never);
    }

    [Fact]
    public async Task GetAllUsersAsync_Should_Query_And_Cache_When_Miss()
    {
        var page = 1;
        var pageSize = 10;
        string? search = null;
        _users.Setup(r => r.CountAsync(search)).ReturnsAsync(2);

        var anyOut = default((IEnumerable<UserResponse>, int));
        _cache.Setup(c => c.TryGetValue(It.IsAny<string>(), out anyOut))
            .Returns(false);

        var user = TestData.User("U1", "u1@t.com");
        _users.Setup(r => r.SearchAsync(search, 0, pageSize))
            .ReturnsAsync(new List<User> { user });

        var service = Service();

        var (users, total) = await service.GetAllUsersAsync(page, pageSize, search);

        total.Should().Be(2);
        users.Single().Name.Should().Be("U1");

        _cache.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<(IEnumerable<UserResponse>, int)>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCoachSubscriptionAsync_Should_Throw_When_Stripe_Data_Exists()
    {
        var coach = TestData.Coach();
        coach.SetStripeData("cus_1", "sub_1");
        _coaches.Setup(r => r.GetByIdAsync(coach.Id)).ReturnsAsync(coach);

        var service = Service();

        var req = new UpdateCoachSubscriptionRequest
        {
            SubscriptionPlan = SubscriptionPlan.Basic,
            SubscriptionStatus = SubscriptionStatus.Active,
            SubscriptionEndAt = DateTime.UtcNow.AddMonths(1)
        };

        var act = () => service.UpdateCoachSubscriptionAsync(coach.Id, req);

        await act.Should().ThrowAsync<ValidationException>();

        _coaches.Verify(r => r.UpdateAsync(It.IsAny<Coach>()), Times.Never);
    }

    [Fact]
    public async Task UpdateCoachSubscriptionAsync_Should_Update_And_Return_Response()
    {
        var coach = TestData.Coach();
        _coaches.Setup(r => r.GetByIdAsync(coach.Id)).ReturnsAsync(coach);
        _coaches.Setup(r => r.UpdateAsync(coach)).Returns(Task.CompletedTask);

        var service = Service();

        var req = new UpdateCoachSubscriptionRequest
        {
            SubscriptionPlan = SubscriptionPlan.Basic,
            SubscriptionStatus = SubscriptionStatus.Active,
            SubscriptionEndAt = DateTime.UtcNow.AddMonths(1)
        };

        var res = await service.UpdateCoachSubscriptionAsync(coach.Id, req);

        res.CoachId.Should().Be(coach.Id);
        res.SubscriptionPlan.Should().Be(req.SubscriptionPlan);
        res.SubscriptionStatus.Should().Be(req.SubscriptionStatus);
        res.SubscriptionEndAt.Should().BeCloseTo(req.SubscriptionEndAt!.Value, TimeSpan.FromSeconds(1));

        _coaches.Verify(r => r.UpdateAsync(coach), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_Should_Delete_User_And_Not_Delete_Coach_When_Not_Found()
    {
        var user = TestData.User("U", "u@test.com");
        _users.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        _coaches.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync((Coach?)null);

        var service = Service();

        await service.DeleteUserAsync(user.Id);

        _users.Verify(r => r.DeleteAsync(user), Times.Once);
        _coaches.Verify(r => r.DeleteAsync(It.IsAny<Coach>()), Times.Never);
    }
}