using FluentAssertions;
using Moq;
using MinhaAcademiaTEM.Application.Caching;
using MinhaAcademiaTEM.Application.DTOs.Users;
using MinhaAcademiaTEM.Application.Services.Coaches;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.Coaches;

public class CoachServiceTests
{
    private readonly Mock<ICurrentUserService> _current = new();
    private readonly Mock<IAppCacheService> _cache = new();
    private readonly Mock<IUserRepository> _users = new();

    private CoachService Service() => new(
        _current.Object, _cache.Object, _users.Object
    );

    [Fact]
    public async Task GetAllCoachClientsAsync_Should_Return_From_Cache_When_Hit_And_No_Search()
    {
        var coachId = Guid.NewGuid();
        var page = 1;
        var pageSize = 10;
        string? search = null;

        _users.Setup(r => r.CountByCoachAsync(coachId, search)).ReturnsAsync(123);

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
}