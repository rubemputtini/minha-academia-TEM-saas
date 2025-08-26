using FluentAssertions;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;

namespace MinhaAcademiaTEM.UnitTests.Domain.Entities;

public class EquipmentSelectionTests
{
    [Fact]
    public void Constructor_Should_Set_UserId_EquipmentId_And_CoachId()
    {
        var coachId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var equipmentId = Guid.NewGuid();

        var selection = TestData.EquipmentSelection(coachId, userId, equipmentId);

        selection.CoachId.Should().Be(coachId);
        selection.UserId.Should().Be(userId);
        selection.EquipmentId.Should().Be(equipmentId);
    }
}