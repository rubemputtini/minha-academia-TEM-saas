using FluentAssertions;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.UnitTests.Domain.Entities;

public class UserTests
{
    private static User NewUser(string name = "Rubem", string email = "rubem@test.com") =>
        new(name, email);

    [Fact]
    public void Constructor_Should_Trim_Name_And_Set_Email_And_UserName()
    {
        var user = NewUser(" Rubem    ");

        user.Name.Should().Be("Rubem");
        user.Email.Should().Be("rubem@test.com");
        user.UserName.Should().Be("rubem@test.com");
    }

    [Fact]
    public void Constructor_Should_Start_With_Empty_EquipmentSelections()
    {
        var user = NewUser();

        user.EquipmentSelections.Should().BeEmpty();
    }

    [Fact]
    public void AssignCoach_Should_Set_CoachId()
    {
        var user = NewUser();
        var coachId = Guid.NewGuid();

        user.AssignCoach(coachId);
        user.CoachId.Should().Be(coachId);
    }

    [Fact]
    public void UpdateName_Should_Trim_And_Update_Name()
    {
        var user = NewUser();
        user.UpdateName("Teste  ");

        user.Name.Should().Be("Teste");
    }
}