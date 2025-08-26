using FluentAssertions;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;

namespace MinhaAcademiaTEM.UnitTests.Domain.Entities;

public class EquipmentNoteTests
{
    [Fact]
    public void Constructor_Should_Set_CoachId_UserId_And_Trim_Message()
    {
        var coachId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var note = TestData.EquipmentNote(coachId, userId, "  Mensagem  ");

        note.CoachId.Should().Be(coachId);
        note.UserId.Should().Be(userId);
        note.Message.Should().Be("Mensagem");
    }

    [Fact]
    public void Update_Should_Trim_And_Update_Message()
    {
        var note = TestData.EquipmentNote(message: "Inicial");
        note.Update("  Nova mensagem  ");
        note.Message.Should().Be("Nova mensagem");
    }

    [Fact]
    public void Update_Should_Set_Empty_When_Whitespace()
    {
        var note = TestData.EquipmentNote(message: "Inicial");
        note.Update("   ");
        note.Message.Should().BeEmpty();
    }

    [Fact]
    public void Update_Should_Not_Change_CoachId_Or_UserId()
    {
        var coachId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var note = TestData.EquipmentNote(coachId, userId, "Oi");

        note.Update("Outra");

        note.CoachId.Should().Be(coachId);
        note.UserId.Should().Be(userId);
    }
}