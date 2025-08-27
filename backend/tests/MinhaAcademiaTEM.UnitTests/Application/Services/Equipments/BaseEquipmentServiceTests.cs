using FluentAssertions;
using Moq;
using MinhaAcademiaTEM.Application.Caching;
using MinhaAcademiaTEM.Application.DTOs.Equipments;
using MinhaAcademiaTEM.Application.Services.Equipments;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.Equipments;

public class BaseEquipmentServiceTests
{
    private readonly Mock<IBaseEquipmentRepository> _repo = new();
    private readonly Mock<IAppCacheService> _cache = new();

    private BaseEquipmentService Service() => new(_repo.Object, _cache.Object);

    [Fact]
    public async Task GetAllAsync_Should_Return_From_Cache_When_Hit()
    {
        var cached = new List<BaseEquipmentResponse>
        {
            new()
            {
                Id = Guid.NewGuid(), Name = "Cached LP", PhotoUrl = "p", VideoUrl = "v",
                MuscleGroup = MuscleGroup.Pernas
            }
        };

        _cache.Setup(c => c.TryGetValue(It.IsAny<string>(), out cached))
            .Returns(true);

        var service = Service();

        var list = await service.GetAllAsync();

        list.Should().HaveCount(1)
            .And.OnlyContain(x => x.Name == "Cached LP");

        _repo.Verify(r => r.GetAllAsync(), Times.Never);
        _cache.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<List<BaseEquipmentResponse>>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_Should_Query_And_Cache_When_Miss()
    {
        List<BaseEquipmentResponse>? any;
        _cache.Setup(c => c.TryGetValue(It.IsAny<string>(), out any))
            .Returns(false);

        var be1 = TestData.BaseEquipment("LP", "p1", "v1", MuscleGroup.Pernas);
        var be2 = TestData.BaseEquipment("CR", "p2", "v2", MuscleGroup.Costas);

        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<BaseEquipment> { be1, be2 });

        var service = Service();

        var list = await service.GetAllAsync();

        list.Should().HaveCount(2);
        list.Should().Contain(x => x.Name == "LP" && x.MuscleGroup == MuscleGroup.Pernas);
        list.Should().Contain(x => x.Name == "CR" && x.MuscleGroup == MuscleGroup.Costas);

        _cache.Verify(c => c.Set(It.IsAny<string>(), It.Is<List<BaseEquipmentResponse>>(l => l.Count == 2)),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Map_And_Return()
    {
        var be = TestData.BaseEquipment("LP", "p", "v", MuscleGroup.Pernas);
        _repo.Setup(r => r.GetByIdAsync(be.Id)).ReturnsAsync(be);

        var service = Service();

        var response = await service.GetByIdAsync(be.Id);

        response.Id.Should().Be(be.Id);
        response.Name.Should().Be("LP");
        response.PhotoUrl.Should().Be("p");
        response.VideoUrl.Should().Be("v");
        response.MuscleGroup.Should().Be(MuscleGroup.Pernas);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Throw_When_NotFound()
    {
        var id = Guid.NewGuid();
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((BaseEquipment?)null);

        var service = Service();

        var act = () => service.GetByIdAsync(id);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_Should_Add_ClearCache_And_Return()
    {
        var request = new CreateBaseEquipmentRequest
        {
            Name = "Novo",
            PhotoUrl = "p",
            VideoUrl = "v",
            MuscleGroup = MuscleGroup.Ombro
        };

        BaseEquipment? created = null;
        _repo.Setup(r => r.AddAsync(It.IsAny<BaseEquipment>()))
            .Callback<BaseEquipment>(e => created = e)
            .Returns(Task.CompletedTask);

        var service = Service();

        var response = await service.CreateAsync(request);

        _repo.Verify(r => r.AddAsync(It.IsAny<BaseEquipment>()), Times.Once);
        _cache.Verify(c => c.Remove(CacheKeys.AllBaseEquipments), Times.Once);

        created.Should().NotBeNull();
        created.Name.Should().Be(request.Name);
        created.PhotoUrl.Should().Be(request.PhotoUrl);
        created.VideoUrl.Should().Be(request.VideoUrl);
        created.MuscleGroup.Should().Be(request.MuscleGroup);

        response.Name.Should().Be(request.Name);
        response.MuscleGroup.Should().Be(request.MuscleGroup);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_And_ClearCache()
    {
        var be = TestData.BaseEquipment("Old", "pOld", "vOld", MuscleGroup.Peito);
        _repo.Setup(r => r.GetByIdAsync(be.Id)).ReturnsAsync(be);
        _repo.Setup(r => r.UpdateAsync(be)).Returns(Task.CompletedTask);

        var request = new UpdateBaseEquipmentRequest
        {
            Name = "New",
            PhotoUrl = "pNew",
            VideoUrl = "vNew",
            MuscleGroup = MuscleGroup.Costas
        };

        var service = Service();

        var response = await service.UpdateAsync(be.Id, request);

        _repo.Verify(r => r.UpdateAsync(be), Times.Once);
        _cache.Verify(c => c.Remove(CacheKeys.AllBaseEquipments), Times.Once);

        be.Name.Should().Be("New");
        be.PhotoUrl.Should().Be("pNew");
        be.VideoUrl.Should().Be("vNew");
        be.MuscleGroup.Should().Be(MuscleGroup.Costas);

        response.Name.Should().Be("New");
        response.PhotoUrl.Should().Be("pNew");
        response.VideoUrl.Should().Be("vNew");
        response.MuscleGroup.Should().Be(MuscleGroup.Costas);
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_NotFound()
    {
        var id = Guid.NewGuid();
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((BaseEquipment?)null);

        var service = Service();

        var act = () => service.UpdateAsync(id, new UpdateBaseEquipmentRequest
        {
            Name = "X",
            PhotoUrl = "p",
            VideoUrl = "v",
            MuscleGroup = MuscleGroup.Pernas
        });

        await act.Should().ThrowAsync<NotFoundException>();
    }


    [Fact]
    public async Task DeleteAsync_Should_Delete_And_ClearCache()
    {
        var be = TestData.BaseEquipment("Del", "p", "v", MuscleGroup.Tríceps);
        _repo.Setup(r => r.GetByIdAsync(be.Id)).ReturnsAsync(be);
        _repo.Setup(r => r.DeleteAsync(be)).Returns(Task.CompletedTask);

        var service = Service();

        await service.DeleteAsync(be.Id);

        _repo.Verify(r => r.DeleteAsync(be), Times.Once);
        _cache.Verify(c => c.Remove(CacheKeys.AllBaseEquipments), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Should_Throw_When_NotFound()
    {
        var id = Guid.NewGuid();
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((BaseEquipment?)null);

        var service = Service();

        var act = () => service.DeleteAsync(id);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}