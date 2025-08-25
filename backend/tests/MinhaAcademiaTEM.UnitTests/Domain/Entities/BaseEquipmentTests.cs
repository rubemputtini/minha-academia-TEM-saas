using FluentAssertions;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.UnitTests.Domain.Entities;

public class BaseEquipmentTests
{
    private static BaseEquipment CreateBaseEquipment(string name = "Leg Press",
        string photoUrl = "https://wwww.youtube.com", string videoUrl = "https://wwww.youtube.com",
        MuscleGroup muscleGroup = MuscleGroup.Pernas) => new(name, photoUrl, videoUrl, muscleGroup);

    [Fact]
    public void Constructor_Should_Trim_Name_PhotoUrl_And_VideoUrl()
    {
        var baseEquipment =
            CreateBaseEquipment("Leg Press  ", "https://wwww.youtube.com    ", "https://wwww.youtube.com  ");

        baseEquipment.Name.Should().Be("Leg Press");
        baseEquipment.PhotoUrl.Should().Be("https://wwww.youtube.com");
        baseEquipment.VideoUrl.Should().Be("https://wwww.youtube.com");
    }

    [Fact]
    public void Constructor_Should_Start_With_Empty_Customizations()
    {
        var baseEquipment = CreateBaseEquipment();

        baseEquipment.Customizations.Should().BeEmpty();
    }

    [Fact]
    public void UpdateName_Should_Trim_And_Update_Name()
    {
        var baseEquipment = CreateBaseEquipment();
        baseEquipment.UpdateName("LEG PRESS  ");

        baseEquipment.Name.Should().Be("LEG PRESS");
    }

    [Fact]
    public void UpdateMedia_Should_Trim_And_Update_PhotoUrl_And_VideoUrl()
    {
        var baseEquipment = CreateBaseEquipment();
        baseEquipment.UpdateMedia("https://wwww.youtube.com.br    ", "https://wwww.youtube.com.br    ");

        baseEquipment.PhotoUrl.Should().Be("https://wwww.youtube.com.br");
        baseEquipment.VideoUrl.Should().Be("https://wwww.youtube.com.br");
    }

    [Fact]
    public void UpdateMuscleGroup_Should_Update_MuscleGroup()
    {
        var baseEquipment = CreateBaseEquipment();
        baseEquipment.UpdateMuscleGroup(MuscleGroup.Abdômen);

        baseEquipment.MuscleGroup.Should().Be(MuscleGroup.Abdômen);
    }
}