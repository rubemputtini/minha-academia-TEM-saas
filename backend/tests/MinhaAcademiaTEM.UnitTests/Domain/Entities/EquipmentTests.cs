using FluentAssertions;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;

namespace MinhaAcademiaTEM.UnitTests.Domain.Entities;

public class EquipmentTests
{
    [Fact]
    public void Constructor_Should_Trim_Name_And_VideoUrl()
    {
        var equipment = TestData.Equipment("Leg Press ", "https://wwww.youtube.com  ", MuscleGroup.Pernas,
            Guid.NewGuid(),
            Guid.NewGuid());

        equipment.Name.Should().Be("Leg Press");
        equipment.VideoUrl.Should().Be("https://wwww.youtube.com");
    }

    [Fact]
    public void Constructor_Should_Set_MuscleGroup_BaseEquipmentId_And_CoachId()
    {
        var baseEquipmentId = Guid.NewGuid();
        var coachId = Guid.NewGuid();

        var equipment = TestData.Equipment(
            name: "Leg Press",
            videoUrl: "https://www.youtube.com",
            muscleGroup: MuscleGroup.Pernas,
            baseEquipmentId: baseEquipmentId,
            coachId: coachId
        );

        equipment.MuscleGroup.Should().Be(MuscleGroup.Pernas);
        equipment.BaseEquipmentId.Should().Be(baseEquipmentId);
        equipment.CoachId.Should().Be(coachId);
    }

    [Fact]
    public void SetActive_Should_Enable_And_Disable()
    {
        var equipment = TestData.Equipment();
        equipment.SetActive(false);

        equipment.IsActive.Should().BeFalse();

        equipment.SetActive(true);
        equipment.IsActive.Should().BeTrue();
    }
}