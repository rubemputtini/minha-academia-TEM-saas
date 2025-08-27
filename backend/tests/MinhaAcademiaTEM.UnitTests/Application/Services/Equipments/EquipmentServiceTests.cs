using FluentAssertions;
using Moq;
using MinhaAcademiaTEM.Application.Caching;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.Equipments;
using MinhaAcademiaTEM.Application.Services.Equipments;
using MinhaAcademiaTEM.Application.Services.Subscriptions;
using MinhaAcademiaTEM.Domain.Constants;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.Equipments;

public class EquipmentServiceTests
{
    private readonly Mock<IEquipmentRepository> _equipmentRepository = new();
    private readonly Mock<IAppCacheService> _cacheService = new();
    private readonly Mock<ICurrentUserService> _currentUser = new();
    private readonly Mock<IUserRepository> _users = new();
    private readonly Mock<ICoachRepository> _coaches = new();
    private readonly Mock<IGymRepository> _gyms = new();
    private readonly Mock<IPlanRulesService> _planRules = new();

    private EquipmentService Service() => new(
        _equipmentRepository.Object,
        _cacheService.Object,
        _currentUser.Object,
        new EntityLookup(_users.Object, _coaches.Object, _gyms.Object),
        access: null!,
        _planRules.Object
    );


    [Fact]
    public async Task GetAllByCoachIdAsync_Should_Return_From_Cache_When_Hit()
    {
        var coachId = Guid.NewGuid();
        var cached = new List<EquipmentResponse>
        {
            new()
            {
                Id = Guid.NewGuid(), Name = "Cadeira Extensora", VideoUrl = "v", MuscleGroup = MuscleGroup.Pernas,
                BaseEquipmentId = Guid.NewGuid(), IsActive = true
            }
        };

        _cacheService.Setup(c => c.TryGetValue(It.IsAny<string>(), out cached))
            .Returns(true);

        var service = Service();

        var response = await service.GetAllByCoachIdAsync(coachId);

        response.Should().HaveCount(1).And.OnlyContain(e => e.Name == "Cadeira Extensora");
        _equipmentRepository.Verify(r => r.GetAllByCoachIdAsync(It.IsAny<Guid>()), Times.Never);
        _cacheService.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<List<EquipmentResponse>>()), Times.Never);
    }

    [Fact]
    public async Task GetAllByCoachIdAsync_Should_Query_And_Cache_When_Miss()
    {
        var coachId = Guid.NewGuid();

        List<EquipmentResponse>? any;
        _cacheService.Setup(c => c.TryGetValue(It.IsAny<string>(), out any))
            .Returns(false);

        var e1 = TestData.Equipment(name: "Supino", videoUrl: "v1", muscleGroup: MuscleGroup.Peito,
            baseEquipmentId: Guid.NewGuid(), coachId: coachId);
        var e2 = TestData.Equipment(name: "Remada", videoUrl: "v2", muscleGroup: MuscleGroup.Costas,
            baseEquipmentId: Guid.NewGuid(), coachId: coachId);
        _equipmentRepository.Setup(r => r.GetAllByCoachIdAsync(coachId))
            .ReturnsAsync(new List<Equipment> { e1, e2 });

        var service = Service();

        var response = await service.GetAllByCoachIdAsync(coachId);

        response.Should().HaveCount(2);
        response.Should().Contain(x => x.Name == "Supino" && x.MuscleGroup == MuscleGroup.Peito);
        response.Should().Contain(x => x.Name == "Remada" && x.MuscleGroup == MuscleGroup.Costas);

        _cacheService.Verify(c => c.Set(It.IsAny<string>(), It.Is<List<EquipmentResponse>>(l => l.Count == 2)),
            Times.Once);
    }

    [Fact]
    public async Task GetAll_Should_Use_CurrentUserId()
    {
        var coachId = Guid.NewGuid();
        _currentUser.Setup(c => c.GetUserId()).Returns(coachId);

        List<EquipmentResponse>? any;
        _cacheService.Setup(c => c.TryGetValue(It.IsAny<string>(), out any))
            .Returns(false);

        var e1 = TestData.Equipment(name: "Agachamento", videoUrl: "v", muscleGroup: MuscleGroup.Pernas,
            baseEquipmentId: Guid.NewGuid(), coachId: coachId);
        _equipmentRepository.Setup(r => r.GetAllByCoachIdAsync(coachId))
            .ReturnsAsync(new List<Equipment> { e1 });

        var service = Service();

        var response = await service.GetAll();

        response.Should().ContainSingle(x => x.Name == "Agachamento");
        _equipmentRepository.Verify(r => r.GetAllByCoachIdAsync(coachId), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Should_Add_And_Invalidate_Cache_And_Return_Response()
    {
        var coachId = Guid.NewGuid();
        _currentUser.Setup(c => c.GetUserId()).Returns(coachId);

        _planRules.Setup(p => p.EnsureCapabilityAsync(coachId, Capability.ManageCustomEquipment))
            .Returns(Task.CompletedTask);

        Equipment? saved = null;
        _equipmentRepository.Setup(r => r.AddAsync(It.IsAny<Equipment>()))
            .Callback<Equipment>(e => saved = e)
            .Returns(Task.CompletedTask);

        var request = new CreateEquipmentRequest
        {
            Name = "Puxada Frente",
            VideoUrl = "video-url",
            MuscleGroup = MuscleGroup.Costas,
            BaseEquipmentId = Guid.NewGuid()
        };

        var service = Service();

        var response = await service.CreateAsync(request);

        _equipmentRepository.Verify(r => r.AddAsync(It.IsAny<Equipment>()), Times.Once);

        var key1 = CacheKeys.CoachEquipments(coachId);
        var key2 = CacheKeys.CoachActiveEquipments(coachId);
        _cacheService.Verify(c => c.RemoveMultiple(It.Is<string[]>(arr =>
            arr.Length == 2 && arr.Contains(key1) && arr.Contains(key2)
        )), Times.Once);

        saved.Should().NotBeNull();
        saved!.CoachId.Should().Be(coachId);
        saved.Name.Should().Be("Puxada Frente");
        saved.VideoUrl.Should().Be("video-url");
        saved.MuscleGroup.Should().Be(MuscleGroup.Costas);
        saved.BaseEquipmentId.Should().Be(request.BaseEquipmentId);

        response.Name.Should().Be(saved.Name);
        response.MuscleGroup.Should().Be(saved.MuscleGroup);
        response.BaseEquipmentId.Should().Be(saved.BaseEquipmentId);
        response.IsActive.Should().Be(saved.IsActive);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Throw_NotFound_When_Equipment_Does_Not_Exist()
    {
        var equipmentId = Guid.NewGuid();
        _equipmentRepository.Setup(r => r.GetByIdAsync(equipmentId))
            .ReturnsAsync((Equipment?)null);

        var service = Service();

        var act = () => service.GetByIdAsync(equipmentId);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_NotFound_When_Equipment_Does_Not_Exist()
    {
        var equipmentId = Guid.NewGuid();
        _currentUser.Setup(c => c.GetUserId()).Returns(Guid.NewGuid());
        _planRules.Setup(p => p.EnsureCapabilityAsync(It.IsAny<Guid>(), Capability.ManageCustomEquipment))
            .Returns(Task.CompletedTask);

        _equipmentRepository.Setup(r => r.GetByIdAsync(equipmentId))
            .ReturnsAsync((Equipment?)null);

        var request = new UpdateEquipmentRequest
        {
            Name = "X",
            VideoUrl = "v",
            MuscleGroup = MuscleGroup.Ombro
        };

        var service = Service();

        var act = () => service.UpdateAsync(equipmentId, request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_Should_Throw_NotFound_When_Equipment_Does_Not_Exist()
    {
        var equipmentId = Guid.NewGuid();
        _currentUser.Setup(c => c.GetUserId()).Returns(Guid.NewGuid());
        _planRules.Setup(p => p.EnsureCapabilityAsync(It.IsAny<Guid>(), Capability.ManageCustomEquipment))
            .Returns(Task.CompletedTask);

        _equipmentRepository.Setup(r => r.GetByIdAsync(equipmentId))
            .ReturnsAsync((Equipment?)null);

        var service = Service();

        var act = () => service.DeleteAsync(equipmentId);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task SetStatusAsync_Should_Throw_NotFound_When_Equipment_Does_Not_Exist()
    {
        var equipmentId = Guid.NewGuid();
        _currentUser.Setup(c => c.GetUserId()).Returns(Guid.NewGuid());
        _planRules.Setup(p => p.EnsureCapabilityAsync(It.IsAny<Guid>(), Capability.ModifyEquipmentStatus))
            .Returns(Task.CompletedTask);

        _equipmentRepository.Setup(r => r.GetByIdAsync(equipmentId))
            .ReturnsAsync((Equipment?)null);

        var request = new ToggleEquipmentRequest { IsActive = false };

        var service = Service();

        var act = () => service.SetStatusAsync(equipmentId, request);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}