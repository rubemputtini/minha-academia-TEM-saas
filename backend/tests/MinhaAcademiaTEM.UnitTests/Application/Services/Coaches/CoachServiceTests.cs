using FluentAssertions;
using Moq;
using MinhaAcademiaTEM.Application.Caching;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.Users;
using MinhaAcademiaTEM.Application.Services.Coaches;
using MinhaAcademiaTEM.Application.Services.Subscriptions;
using MinhaAcademiaTEM.Domain.Constants;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.Coaches;

public class CoachServiceTests
{
    private readonly Mock<ICurrentUserService> _current = new();
    private readonly Mock<IAppCacheService> _cache = new();
    private readonly Mock<IUserRepository> _users = new();
    private readonly Mock<ICoachRepository> _coaches = new();
    private readonly Mock<IGymRepository> _gyms = new();
    private readonly Mock<IPlanRulesService> _planRules = new();

    private CoachService Service()
    {
        var lookup = new EntityLookup(_users.Object, _coaches.Object, _gyms.Object);
        return new(_current.Object, _cache.Object, _users.Object, lookup, _planRules.Object);
    }

    [Fact]
    public async Task GetAllCoachClientsAsync_Should_Return_From_Cache_When_Hit_And_No_Search()
    {
        var coachId = Guid.NewGuid();
        var page = 1;
        var pageSize = 10;
        string? search = null;

        var cached = (Clients: (IEnumerable<UserResponse>)new[]
        {
            new UserResponse
                { Id = Guid.NewGuid(), Name = "Cached", Email = "c@t.com", CoachId = coachId, CoachName = "Coach" }
        }, TotalClients: 123);

        _cache.Setup(c => c.TryGetValue<(IEnumerable<UserResponse>, int)>(It.IsAny<string>(), out cached))
            .Returns(true);

        var service = Service();

        var (clients, total) = await service.GetAllCoachClientsAsync(coachId, page, pageSize, search);

        total.Should().Be(123);
        clients.Should().ContainSingle(x => x.Name == "Cached");

        _users.Verify(r => r.CountByCoachAsync(It.IsAny<Guid>(), It.IsAny<string?>()), Times.Never);
        _users.Verify(
            r => r.SearchByCoachAsync(It.IsAny<Guid>(), It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<int>()),
            Times.Never);
        _cache.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<(IEnumerable<UserResponse>, int)>()), Times.Never);
    }

    [Fact]
    public async Task GetAllCoachClientsAsync_Should_Query_And_Cache_When_Miss_And_No_Search()
    {
        var coachId = Guid.NewGuid();
        var page = 2;
        var pageSize = 5;
        string? search = null;

        _users.Setup(r => r.CountByCoachAsync(coachId, search)).ReturnsAsync(0);

        (IEnumerable<UserResponse>, int) any;
        _cache.Setup(c => c.TryGetValue(It.IsAny<string>(), out any)).Returns(false);

        _users.Setup(r => r.SearchByCoachAsync(coachId, search, (page - 1) * pageSize, pageSize))
            .ReturnsAsync(new List<User>());

        var service = Service();

        var (clients, total) = await service.GetAllCoachClientsAsync(coachId, page, pageSize, search);

        total.Should().Be(0);
        clients.Should().BeEmpty();

        _users.Verify(r => r.SearchByCoachAsync(coachId, search, (page - 1) * pageSize, pageSize), Times.Once);
        _cache.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<(IEnumerable<UserResponse>, int)>()), Times.Once);
    }

    [Fact]
    public async Task GetAllCoachClientsAsync_Should_Ignore_Cache_When_SearchTerm()
    {
        var coachId = Guid.NewGuid();
        var page = 1;
        var pageSize = 10;
        var search = "ana";

        _users.Setup(r => r.CountByCoachAsync(coachId, search)).ReturnsAsync(0);

        (IEnumerable<UserResponse>, int) fake;
        _cache.Setup(c => c.TryGetValue(It.IsAny<string>(), out fake)).Returns(true);

        _users.Setup(r => r.SearchByCoachAsync(coachId, search, 0, pageSize))
            .ReturnsAsync(new List<User>());

        var service = Service();

        var (clients, total) = await service.GetAllCoachClientsAsync(coachId, page, pageSize, search);

        total.Should().Be(0);
        clients.Should().BeEmpty();

        _cache.Verify(c => c.TryGetValue(It.IsAny<string>(), out fake), Times.Never);
        _cache.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<(IEnumerable<UserResponse>, int)>()), Times.Never);
        _users.Verify(r => r.SearchByCoachAsync(coachId, search, 0, pageSize), Times.Once);
    }

    [Fact]
    public async Task GetOwnClientsAsync_Should_Use_CurrentUserId()
    {
        var coachId = Guid.NewGuid();
        _current.Setup(c => c.GetUserId()).Returns(coachId);

        var page = 3;
        var pageSize = 4;
        string? search = null;

        _users.Setup(r => r.CountByCoachAsync(coachId, search)).ReturnsAsync(0);

        (IEnumerable<UserResponse>, int) miss;
        _cache.Setup(c => c.TryGetValue(It.IsAny<string>(), out miss)).Returns(false);

        _users.Setup(r => r.SearchByCoachAsync(coachId, search, (page - 1) * pageSize, pageSize))
            .ReturnsAsync(new List<User>());

        var service = Service();

        var (clients, total) = await service.GetOwnClientsAsync(page, pageSize, search);

        total.Should().Be(0);
        clients.Should().BeEmpty();

        _users.Verify(r => r.SearchByCoachAsync(coachId, search, (page - 1) * pageSize, pageSize), Times.Once);
    }

    [Fact]
    public async Task GetTotalClientsAsync_Should_Use_CurrentUserId()
    {
        var coachId = Guid.NewGuid();
        _current.Setup(c => c.GetUserId()).Returns(coachId);

        _users.Setup(r => r.CountByCoachAsync(coachId, null)).ReturnsAsync(7);

        var service = Service();

        var total = await service.GetTotalClientsAsync();

        total.Should().Be(7);
        _users.Verify(r => r.CountByCoachAsync(coachId, null), Times.Once);
    }

    [Fact]
    public async Task DeleteCoachClientAsync_Should_Delete_When_Belongs_To_Current_Coach()
    {
        var coachId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _current.Setup(c => c.GetUserId()).Returns(coachId);

        var user = new User("U", "u@t.com") { Id = userId };
        user.AssignCoach(coachId);

        _users.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _users.Setup(r => r.DeleteAsync(user)).Returns(Task.CompletedTask);

        var service = Service();

        await service.DeleteCoachClientAsync(userId);

        _users.Verify(r => r.DeleteAsync(user), Times.Once);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task DeleteCoachClientAsync_Should_Throw_NotFound_When_NotFound_Or_NotOwned(bool notFound)
    {
        var myCoachId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _current.Setup(c => c.GetUserId()).Returns(myCoachId);

        if (notFound)
        {
            _users.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User?)null);
        }
        else
        {
            var otherCoachId = Guid.NewGuid();
            var user = new User("X", "x@t.com") { Id = userId };
            user.AssignCoach(otherCoachId);
            _users.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        }

        var service = Service();

        var act = () => service.DeleteCoachClientAsync(userId);

        await act.Should().ThrowAsync<NotFoundException>();

        _users.Verify(r => r.DeleteAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task SetClientActiveAsync_Should_Update_And_Invalidate_Cache_When_Valid()
    {
        var coachId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var user = TestData.User(id: userId, coachId: coachId);

        _current.Setup(c => c.GetUserId()).Returns(coachId);
        _planRules.Setup(p => p.EnsureCapabilityAsync(coachId, It.IsAny<Capability>())).Returns(Task.CompletedTask);
        _users.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _users.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);

        var service = Service();

        await service.SetClientActiveAsync(userId, false);

        user.IsActive.Should().BeFalse();
        _users.Verify(r => r.UpdateAsync(user), Times.Once);
        _cache.Verify(c => c.RemoveByPrefix(It.IsAny<string>()), Times.AtLeastOnce);
        _cache.Verify(c => c.Remove(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task SetClientActiveAsync_Should_Throw_Forbidden_When_Client_Belongs_To_Other_Coach()
    {
        var myCoachId = Guid.NewGuid();
        var otherCoachId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var user = TestData.User(id: userId, coachId: otherCoachId);

        _current.Setup(c => c.GetUserId()).Returns(myCoachId);
        _planRules.Setup(p => p.EnsureCapabilityAsync(myCoachId, It.IsAny<Capability>())).Returns(Task.CompletedTask);
        _users.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

        var service = Service();

        var act = () => service.SetClientActiveAsync(userId, true);

        await act.Should().ThrowAsync<ForbiddenException>();
        _users.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task UpdateClientTrainingDateAsync_Should_Update_And_Invalidate_Cache_When_Valid()
    {
        var coachId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var user = TestData.User(id: userId, coachId: coachId);
        var newDate = DateTime.UtcNow.AddDays(30);

        _current.Setup(c => c.GetUserId()).Returns(coachId);
        _users.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _users.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);

        var service = Service();

        await service.UpdateClientTrainingDateAsync(userId, newDate);

        user.NextTrainingChangeAt.Should().BeCloseTo(newDate, TimeSpan.FromSeconds(1));
        _users.Verify(r => r.UpdateAsync(user), Times.Once);
        _cache.Verify(c => c.RemoveByPrefix(It.IsAny<string>()), Times.AtLeastOnce);
        _cache.Verify(c => c.Remove(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task UpdateClientTrainingDateAsync_Should_Throw_NotFound_When_Client_Not_Owned()
    {
        var myCoachId = Guid.NewGuid();
        var otherCoachId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var user = TestData.User(id: userId, coachId: otherCoachId);

        _current.Setup(c => c.GetUserId()).Returns(myCoachId);
        _users.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

        var service = Service();

        var act = () => service.UpdateClientTrainingDateAsync(userId, DateTime.UtcNow);

        await act.Should().ThrowAsync<NotFoundException>();
        _users.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
    }
}