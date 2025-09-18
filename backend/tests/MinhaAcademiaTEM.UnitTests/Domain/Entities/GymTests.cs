using FluentAssertions;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;

namespace MinhaAcademiaTEM.UnitTests.Domain.Entities;

public class GymTests
{
    [Fact]
    public void Constructor_Should_Trim_Name_And_Location()
    {
        var gym = TestData.Gym(Guid.NewGuid(), "Academia  ", "Porto ", "Portugal  ", Guid.NewGuid());

        gym.Name.Should().Be("Academia");
        gym.City.Should().Be("Porto");
        gym.Country.Should().Be("Portugal");
    }

    [Fact]
    public void Constructor_Should_Set_UserId_And_CoachId()
    {
        var coachId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var gym = TestData.Gym(coachId, "Academia", "Porto", "Portugal", userId);

        gym.CoachId.Should().Be(coachId);
        gym.UserId.Should().Be(userId);
    }

    [Fact]
    public void UpdateInfo_Should_Trim_And_Update_Name_And_Location()
    {
        var gym = TestData.Gym(Guid.NewGuid(), "Academia", "Porto", "Portugal", Guid.NewGuid());

        gym.UpdateInfo("ACADEMIA ", "BRASILIA ","BRASIL ");
        gym.Name.Should().Be("ACADEMIA");
        gym.City.Should().Be("BRASILIA");
        gym.Country.Should().Be("BRASIL");
    }
}