using FluentAssertions;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.UnitTests.Domain.Entities;

public class EquipmentTests
{
    private static Equipment CreateEquipment(string name = "Leg Press", string videoUrl = "https://wwww.youtube.com",
        MuscleGroup muscleGroup = MuscleGroup.Pernas,
        Guid baseEquipmentId = default, Guid coachId = default) =>
        new(name, videoUrl, muscleGroup, baseEquipmentId, coachId);

    [Fact]
    public void Constructor_Should_Trim_Name_And_VideoUrl()
    {
        var equipment = CreateEquipment("Leg Press ", "https://wwww.youtube.com  ", MuscleGroup.Pernas, Guid.NewGuid(),
            Guid.NewGuid());

        equipment.Name.Should().Be("Leg Press");
        equipment.VideoUrl.Should().Be("https://wwww.youtube.com");
    }

    [Fact]
    public void Constructor_Should_Set_MuscleGroup_BaseEquipmentId_And_CoachId()
    {
        var baseEquipmentId = Guid.NewGuid();
        var coachId = Guid.NewGuid();

        var equipment = CreateEquipment(
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
        var equipment = CreateEquipment();
        equipment.SetActive(false);

        equipment.IsActive.Should().BeFalse();

        equipment.SetActive(true);
        equipment.IsActive.Should().BeTrue();
    }
}