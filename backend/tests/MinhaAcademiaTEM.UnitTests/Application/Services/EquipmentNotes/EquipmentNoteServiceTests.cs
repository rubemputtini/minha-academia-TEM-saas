using FluentAssertions;
using Moq;
using MinhaAcademiaTEM.Application.Caching;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.EquipmentNotes;
using MinhaAcademiaTEM.Application.Services.EquipmentNotes;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.EquipmentNotes;

public class EquipmentNoteServiceTests
{
    private readonly Mock<IUserRepository> _users = new();
    private readonly Mock<ICoachRepository> _coaches = new();
    private readonly Mock<IGymRepository> _gyms = new();
    private readonly Mock<IEquipmentNoteRepository> _equipmentNotes = new();
    private readonly Mock<ICurrentUserService> _currentUser = new();
    private readonly Mock<IAppCacheService> _cache = new();

    private EquipmentNoteService CreateService() => new(
        new EntityLookup(_users.Object, _coaches.Object, _gyms.Object),
        access: null!,
        _equipmentNotes.Object,
        _currentUser.Object,
        _cache.Object
    );

    [Fact]
    public async Task GetByUserIdAsync_Should_Return_From_Cache_When_Hit()
    {
        var userId = Guid.NewGuid();
        var cached = new EquipmentNoteResponse { Message = "Cached note" };

        _cache.Setup(c => c.TryGetValue(It.IsAny<string>(), out cached))
            .Returns(true);

        var service = CreateService();

        var response = await service.GetByUserIdAsync(userId);

        response.Message.Should().Be("Cached note");

        _users.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _coaches.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _equipmentNotes.Verify(r => r.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        _cache.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<EquipmentNoteResponse>()), Times.Never);
    }

    [Fact]
    public async Task UpsertAsync_Should_Add_When_NotExists_And_Clear_Cache()
    {
        var userId = Guid.NewGuid();
        var coachId = Guid.NewGuid();

        _currentUser.Setup(c => c.GetUserId()).Returns(userId);

        var user = TestData.User("U", "u@t.com", id: userId, coachId: coachId);
        var coach = TestData.Coach(id: coachId, name: "Coach");

        _users.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _coaches.Setup(r => r.GetByUserIdAsync(coachId)).ReturnsAsync(coach);
        _equipmentNotes.Setup(r => r.GetByUserIdAsync(userId, coachId)).ReturnsAsync((EquipmentNote?)null);

        EquipmentNote? created = null;
        _equipmentNotes.Setup(r => r.AddAsync(It.IsAny<EquipmentNote>()))
            .Callback<EquipmentNote>(n => created = n)
            .Returns(Task.CompletedTask);

        var request = new UpsertEquipmentNoteRequest { Message = "  Novo recado  " };

        var service = CreateService();

        var response = await service.UpsertAsync(request);

        _equipmentNotes.Verify(r => r.AddAsync(It.IsAny<EquipmentNote>()), Times.Once);
        _equipmentNotes.Verify(r => r.UpdateAsync(It.IsAny<EquipmentNote>()), Times.Never);

        _cache.Verify(c => c.Remove(CacheKeys.UserEquipmentNotes(userId)), Times.Once);

        created.Should().NotBeNull();
        created!.UserId.Should().Be(userId);
        created.CoachId.Should().Be(coachId);
        created.Message.Should().Be(request.Message.Trim());

        response.Message.Should().Be(request.Message);
    }

    [Fact]
    public async Task UpsertAsync_Should_Update_When_Exists_And_Clear_Cache()
    {
        var userId = Guid.NewGuid();
        var coachId = Guid.NewGuid();

        _currentUser.Setup(c => c.GetUserId()).Returns(userId);

        var user = TestData.User("U", "u@t.com", id: userId, coachId: coachId);
        var coach = TestData.Coach(id: coachId, name: "Coach");

        _users.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _coaches.Setup(r => r.GetByUserIdAsync(coachId)).ReturnsAsync(coach);

        var existing = TestData.EquipmentNote(coachId: coachId, userId: userId, message: "old");
        _equipmentNotes.Setup(r => r.GetByUserIdAsync(userId, coachId)).ReturnsAsync(existing);
        _equipmentNotes.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);

        var request = new UpsertEquipmentNoteRequest { Message = "updated text" };

        var service = CreateService();

        var response = await service.UpsertAsync(request);

        _equipmentNotes.Verify(r => r.AddAsync(It.IsAny<EquipmentNote>()), Times.Never);
        _equipmentNotes.Verify(
            r => r.UpdateAsync(It.Is<EquipmentNote>(n => n == existing && n.Message == "updated text")), Times.Once);

        _cache.Verify(c => c.Remove(CacheKeys.UserEquipmentNotes(userId)), Times.Once);

        response.Message.Should().Be("updated text");
    }
}