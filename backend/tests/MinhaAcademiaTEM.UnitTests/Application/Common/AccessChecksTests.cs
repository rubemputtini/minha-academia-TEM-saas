using FluentAssertions;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;
using Moq;

namespace MinhaAcademiaTEM.UnitTests.Application.Common;

public class AccessChecksTests
{
    private static AccessChecks CheckAs(Guid currentId)
    {
        var current = new Mock<ICurrentUserService>();
        current.Setup(c => c.GetUserId()).Returns(currentId);

        return new AccessChecks(current.Object);
    }

    [Fact]
    public void EnsureCurrentCoachOwnsUser_Should_Pass_When_Current_Is_Coach_And_User_Belongs_To_Coach()
    {
        var coach = TestData.Coach();
        var user = TestData.User(id: Guid.NewGuid(), coachId: coach.Id);

        var checks = CheckAs(coach.Id);

        var action = () => checks.EnsureCurrentCoachOwnsUser(coach, user);

        action.Should().NotThrow();
    }

    [Fact]
    public void EnsureCurrentCoachOwnsUser_Should_Throw_When_User_Does_Not_Belong_To_Coach()
    {
        var coach = TestData.Coach();
        var user = TestData.User(id: Guid.NewGuid(), coachId: Guid.NewGuid());

        var checks = CheckAs(coach.Id);

        var action = () => checks.EnsureCurrentCoachOwnsUser(coach, user);

        action.Should().Throw<ForbiddenException>();
    }

    [Fact]
    public void EnsureCurrentCoachOwnsUser_Should_Throw_When_Current_Is_Not_The_Coach()
    {
        var coach = TestData.Coach();
        var user = TestData.User(id: Guid.NewGuid(), coachId: coach.Id);

        var checks = CheckAs(Guid.NewGuid());

        var action = () => checks.EnsureCurrentCoachOwnsUser(coach, user);

        action.Should().Throw<ForbiddenException>();
    }

    [Fact]
    public void EnsureCurrentUserHasPermission_Should_Pass_When_Current_Is_Coach()
    {
        var coach = TestData.Coach();
        var user = TestData.User(id: coach.Id, coachId: coach.Id);

        var checks = CheckAs(coach.Id);

        var action = () => checks.EnsureCurrentUserHasPermission(user, coach);

        action.Should().NotThrow();
    }

    [Fact]
    public void EnsureCurrentUserHasPermission_Should_Pass_When_User_Is_Client_Of_Coach()
    {
        var coach = TestData.Coach();
        var user = TestData.User(id: Guid.NewGuid(), coachId: coach.Id);

        var checks = CheckAs(coach.Id);

        var action = () => checks.EnsureCurrentUserHasPermission(user, coach);

        action.Should().NotThrow();
    }

    [Fact]
    public void EnsureCanView_Should_Pass_When_Current_Is_Coach_Owner()
    {
        var coach = TestData.Coach();
        var equipment = TestData.Equipment(coachId: coach.Id);
        var user = TestData.User(coachId: coach.Id);

        var checks = CheckAs(coach.Id);

        var action = () => checks.EnsureCanView(equipment, user);

        action.Should().NotThrow();
    }

    [Fact]
    public void EnsureCanView_Should_Throw_When_No_Permission()
    {
        var coach = TestData.Coach();
        var equipment = TestData.Equipment();
        var user = TestData.User();

        var checks = CheckAs(coach.Id);

        var action = () => checks.EnsureCanView(equipment, user);

        action.Should().Throw<ForbiddenException>();
    }

    [Fact]
    public void EnsureCurrentCoachOwns_Should_Throw_When_Current_Is_Not_Owner()
    {
        var coach = TestData.Coach();
        var equipment = TestData.Equipment();

        var checks = CheckAs(coach.Id);

        var action = () => checks.EnsureCurrentCoachOwns(equipment);

        action.Should().Throw<ForbiddenException>();
    }

    [Fact]
    public void EnsureSelfOrCoachOf_Should_Throw_When_Not_Self_And_Not_Coach()
    {
        var coach = TestData.Coach();
        var user = TestData.User();

        var checks = CheckAs(coach.Id);

        var action = () => checks.EnsureSelfOrCoachOf(user);

        action.Should().Throw<ForbiddenException>();
    }
}