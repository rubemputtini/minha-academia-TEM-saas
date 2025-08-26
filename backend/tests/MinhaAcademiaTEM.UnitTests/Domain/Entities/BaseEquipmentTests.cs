using FluentAssertions;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;

namespace MinhaAcademiaTEM.UnitTests.Domain.Entities;

public class BaseEquipmentTests
{
    [Fact]
    public void Constructor_Should_Trim_Name_PhotoUrl_And_VideoUrl()
    {
        var baseEquipment =
            TestData.BaseEquipment("Leg Press  ", "https://wwww.youtube.com    ", "https://wwww.youtube.com  ");

        baseEquipment.Name.Should().Be("Leg Press");
        baseEquipment.PhotoUrl.Should().Be("https://wwww.youtube.com");
        baseEquipment.VideoUrl.Should().Be("https://wwww.youtube.com");
    }

    [Fact]
    public void Constructor_Should_Start_With_Empty_Customizations()
    {
        var baseEquipment = TestData.BaseEquipment();

        baseEquipment.Customizations.Should().BeEmpty();
    }

    [Fact]
    public void UpdateName_Should_Trim_And_Update_Name()
    {
        var baseEquipment = TestData.BaseEquipment();
        baseEquipment.UpdateName("LEG PRESS  ");

        baseEquipment.Name.Should().Be("LEG PRESS");
    }

    [Fact]
    public void UpdateMedia_Should_Trim_And_Update_PhotoUrl_And_VideoUrl()
    {
        var baseEquipment = TestData.BaseEquipment();
        baseEquipment.UpdateMedia("https://wwww.youtube.com.br    ", "https://wwww.youtube.com.br    ");

        baseEquipment.PhotoUrl.Should().Be("https://wwww.youtube.com.br");
        baseEquipment.VideoUrl.Should().Be("https://wwww.youtube.com.br");
    }

    [Fact]
    public void UpdateMuscleGroup_Should_Update_MuscleGroup()
    {
        var baseEquipment = TestData.BaseEquipment();
        baseEquipment.UpdateMuscleGroup(MuscleGroup.Abdômen);

        baseEquipment.MuscleGroup.Should().Be(MuscleGroup.Abdômen);
    }
}