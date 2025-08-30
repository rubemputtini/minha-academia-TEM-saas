using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Security;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;

namespace MinhaAcademiaTEM.UnitTests.Infrastructure.Security;

public class CoachHasAccessHandlerTests
{
    private static ClaimsPrincipal PrincipalWithRoles(params string[] roles)
    {
        var claims = roles.Select(r => new Claim(ClaimTypes.Role, r));
        var identity = new ClaimsIdentity(claims, authenticationType: "TestAuth");
        
        return new ClaimsPrincipal(identity);
    }

    private static AuthorizationHandlerContext Ctx(IAuthorizationRequirement req, ClaimsPrincipal user) =>
        new(new[] { req }, user, resource: null);

    private readonly Mock<ICurrentUserService> _currentUser = new();
    private readonly Mock<IUserRepository> _users = new();
    private readonly Mock<ICoachRepository> _coaches = new();
    private readonly Mock<IGymRepository> _gyms = new();

    private EntityLookup Lookup() => new(_users.Object, _coaches.Object, _gyms.Object);

    [Fact]
    public async Task Admin_Should_Succeed_Without_Repo_Access()
    {
        var requirement = new CoachHasAccessRequirement();
        var context = Ctx(requirement, PrincipalWithRoles(nameof(UserRole.Admin)));

        var handler = new CoachHasAccessHandler(_currentUser.Object, Lookup());

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
        _coaches.Verify(r => r.GetByUserIdAsync(It.IsAny<Guid>()), Times.Never);
        _currentUser.Verify(c => c.GetUserId(), Times.Never);
    }

    [Fact]
    public async Task NonCoach_Should_Not_Succeed_And_Not_Call_Lookup()
    {
        var requirement = new CoachHasAccessRequirement();
        var user = PrincipalWithRoles("SomeOtherRole");
        var context = Ctx(requirement, user);

        var handler = new CoachHasAccessHandler(_currentUser.Object, Lookup());

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
        context.HasFailed.Should().BeFalse();
        _coaches.Verify(r => r.GetByUserIdAsync(It.IsAny<Guid>()), Times.Never);
        _currentUser.Verify(c => c.GetUserId(), Times.Never);
    }

    [Fact]
    public async Task Coach_With_Access_Should_Succeed()
    {
        var requirement = new CoachHasAccessRequirement();
        var context = Ctx(requirement, PrincipalWithRoles(nameof(UserRole.Coach)));

        var userId = Guid.NewGuid();
        _currentUser.Setup(c => c.GetUserId()).Returns(userId);

        var coach = TestData.Coach();
        coach.SetSubscription(coach.SubscriptionPlan, SubscriptionStatus.Active, null); 

        _coaches.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(coach);

        var handler = new CoachHasAccessHandler(_currentUser.Object, Lookup());

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
        _coaches.Verify(r => r.GetByUserIdAsync(userId), Times.Once);
        _currentUser.Verify(c => c.GetUserId(), Times.Once);
    }

    [Fact]
    public async Task Coach_Without_Access_Should_Fail()
    {
        var requirement = new CoachHasAccessRequirement();
        var context = Ctx(requirement, PrincipalWithRoles(nameof(UserRole.Coach)));

        var userId = Guid.NewGuid();
        _currentUser.Setup(c => c.GetUserId()).Returns(userId);

        var coach = TestData.Coach();
        coach.SetSubscription(coach.SubscriptionPlan, SubscriptionStatus.Canceled, null);

        _coaches.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(coach);

        var handler = new CoachHasAccessHandler(_currentUser.Object, Lookup());

        await handler.HandleAsync(context);

        context.HasFailed.Should().BeTrue();
        context.HasSucceeded.Should().BeFalse();
        _coaches.Verify(r => r.GetByUserIdAsync(userId), Times.Once);
        _currentUser.Verify(c => c.GetUserId(), Times.Once);
    }
}