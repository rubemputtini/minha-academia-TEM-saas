using FluentAssertions;
using Moq;
using MinhaAcademiaTEM.Application.Caching;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.EquipmentSelections;
using MinhaAcademiaTEM.Application.Services.EquipmentSelections;
using MinhaAcademiaTEM.Application.Services.Subscriptions;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.EquipmentSelections;

public class EquipmentSelectionServiceTests
{
    private readonly Mock<IUserRepository> _users = new();
    private readonly Mock<ICoachRepository> _coaches = new();
    private readonly Mock<IGymRepository> _gyms = new();
    private readonly Mock<IEquipmentRepository> _equipmentRepository = new();
    private readonly Mock<IEquipmentSelectionRepository> _equipmentSelectionRepository = new();
    private readonly Mock<IAppCacheService> _cacheService = new();
    private readonly Mock<ICurrentUserService> _currentUser = new();
    private readonly Mock<IPlanRulesService> _planRules = new();

    private EquipmentSelectionService Service() => new(
        new EntityLookup(_users.Object, _coaches.Object, _gyms.Object),
        access: null!,
        _equipmentRepository.Object,
        _equipmentSelectionRepository.Object,
        _cacheService.Object,
        _currentUser.Object,
        _planRules.Object
    );

    [Fact]
    public async Task GetUserViewAsync_Should_Return_From_Cache_When_Hit()
    {
        var userId = Guid.NewGuid();
        var cached = new List<UserEquipmentItemResponse>
        {
            new()
            {
                EquipmentId = Guid.NewGuid(), Name = "Agachamento", VideoUrl = "v", MuscleGroup = MuscleGroup.Pernas,
                IsAvailable = true
            }
        };

        _cacheService.Setup(c => c.TryGetValue(It.IsAny<string>(), out cached))
            .Returns(true);

        var service = Service();

        var response = await service.GetUserViewAsync(userId);

        response.Should().HaveCount(1).And.OnlyContain(x => x.Name == "Agachamento");

        _users.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _coaches.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _equipmentRepository.Verify(r => r.GetActiveByCoachIdAsync(It.IsAny<Guid>()), Times.Never);
        _equipmentSelectionRepository.Verify(r => r.GetByUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        _cacheService.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<List<UserEquipmentItemResponse>>()), Times.Never);
    }

    [Fact]
    public async Task GetUserViewAsync_Should_Query_Map_And_Cache_When_Miss()
    {
        var userId = Guid.NewGuid();
        List<UserEquipmentItemResponse>? any;
        _cacheService.Setup(c => c.TryGetValue(It.IsAny<string>(), out any))
            .Returns(false);

        var coach = TestData.Coach(name: "Coach X");
        var user = TestData.User("User", "u@t.com", id: userId, coachId: coach.Id);
        _users.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _coaches.Setup(r => r.GetByIdAsync(coach.Id)).ReturnsAsync(coach);

        var e1 = TestData.Equipment(name: "Supino", videoUrl: "v1", muscleGroup: MuscleGroup.Peito,
            baseEquipmentId: Guid.NewGuid(), coachId: coach.Id);
        var e2 = TestData.Equipment(name: "Remada", videoUrl: "v2", muscleGroup: MuscleGroup.Costas,
            baseEquipmentId: Guid.NewGuid(), coachId: coach.Id);
        var equipments = new List<Equipment> { e1, e2 };
        _equipmentRepository.Setup(r => r.GetActiveByCoachIdAsync(coach.Id)).ReturnsAsync(equipments);

        var selections = new List<EquipmentSelection>
        {
            new(coach.Id, userId, e2.Id)
        };
        _equipmentSelectionRepository.Setup(r => r.GetByUserAsync(coach.Id, userId)).ReturnsAsync(selections);

        var service = Service();

        var response = await service.GetUserViewAsync(userId);

        response.Should().HaveCount(2);
        response.Should().Contain(x => x.EquipmentId == e1.Id && x.Name == "Supino" && x.IsAvailable == false);
        response.Should().Contain(x => x.EquipmentId == e2.Id && x.Name == "Remada" && x.IsAvailable == true);

        _cacheService.Verify(c => c.Set(It.IsAny<string>(), It.Is<List<UserEquipmentItemResponse>>(l => l.Count == 2)),
            Times.Once);
    }

    [Fact]
    public async Task GetCoachViewAsync_Should_Return_From_Cache_When_Hit()
    {
        var userId = Guid.NewGuid();
        var cached = new List<CoachEquipmentItemResponse>
        {
            new() { EquipmentId = Guid.NewGuid(), Name = "Crossover", VideoUrl = "v", MuscleGroup = MuscleGroup.Peito }
        };

        _cacheService.Setup(c => c.TryGetValue(It.IsAny<string>(), out cached))
            .Returns(true);

        var service = Service();

        var response = await service.GetCoachViewAsync(userId);

        response.Should().HaveCount(1).And.OnlyContain(x => x.Name == "Crossover");

        _users.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _coaches.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _equipmentRepository.Verify(r => r.GetActiveByCoachIdAsync(It.IsAny<Guid>()), Times.Never);
        _equipmentSelectionRepository.Verify(r => r.GetByUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        _cacheService.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<List<CoachEquipmentItemResponse>>()), Times.Never);
    }
}