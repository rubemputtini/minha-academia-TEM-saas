using FluentAssertions;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.UnitTests.Domain.Entities;

public class EquipmentSelectionTests
{
    private static EquipmentSelection NewEquipmentSelection(Guid coachId, Guid userId, Guid equipmentId) =>
        new(coachId, userId, equipmentId);

    [Fact]
    public void Constructor_Should_Set_UserId_EquipmentId_And_CoachId()
    {
        var coachId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var equipmentId = Guid.NewGuid();

        var selection = NewEquipmentSelection(coachId, userId, equipmentId);

        selection.CoachId.Should().Be(coachId);
        selection.UserId.Should().Be(userId);
        selection.EquipmentId.Should().Be(equipmentId);
    }
}