using FluentAssertions;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;

namespace MinhaAcademiaTEM.UnitTests.Domain.Entities;

public class UserTests
{
    [Fact]
    public void Constructor_Should_Trim_Name_And_Set_Email_And_UserName()
    {
        var user = TestData.User(" Rubem    ");

        user.Name.Should().Be("Rubem");
        user.Email.Should().Be("user@test.com");
        user.UserName.Should().Be("user@test.com");
    }

    [Fact]
    public void Constructor_Should_Start_With_Empty_EquipmentSelections()
    {
        var user = TestData.User();

        user.EquipmentSelections.Should().BeEmpty();
    }

    [Fact]
    public void AssignCoach_Should_Set_CoachId()
    {
        var user = TestData.User();
        var coachId = Guid.NewGuid();

        user.AssignCoach(coachId);
        user.CoachId.Should().Be(coachId);
    }

    [Fact]
    public void UpdateName_Should_Trim_And_Update_Name()
    {
        var user = TestData.User();
        user.UpdateName("Teste  ");

        user.Name.Should().Be("Teste");
    }
}