using FluentAssertions;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.UnitTests.Domain.Entities;

public class GymTests
{
    private static Gym NewGym(Guid coachId, string name, string location, Guid userId) =>
        new(coachId, name, location, userId);

    [Fact]
    public void Constructor_Should_Trim_Name_And_Location()
    {
        var gym = NewGym(Guid.NewGuid(), "Academia  ", "Portugal  ", Guid.NewGuid());

        gym.Name.Should().Be("Academia");
        gym.Location.Should().Be("Portugal");
    }

    [Fact]
    public void Constructor_Should_Set_UserId_And_CoachId()
    {
        var coachId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var gym = NewGym(coachId, "Academia", "Portugal", userId);

        gym.CoachId.Should().Be(coachId);
        gym.UserId.Should().Be(userId);
    }

    [Fact]
    public void UpdateInfo_Should_Trim_And_Update_Name_And_Location()
    {
        var gym = NewGym(Guid.NewGuid(), "Academia", "Portugal", Guid.NewGuid());

        gym.UpdateInfo("ACADEMIA ", "BRASIL ");
        gym.Name.Should().Be("ACADEMIA");
        gym.Location.Should().Be("BRASIL");
    }
}