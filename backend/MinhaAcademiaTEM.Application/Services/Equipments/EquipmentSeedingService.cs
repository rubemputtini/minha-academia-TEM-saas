using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Application.Services.Equipments;

public class EquipmentSeedingService(
    IBaseEquipmentRepository baseEquipmentRepository,
    IEquipmentRepository equipmentRepository)
    : IEquipmentSeedingService
{
    public async Task SeedForCoachAsync(Guid coachId)
    {
        var baseEquipments = await baseEquipmentRepository.GetAllAsync();

        if (!baseEquipments.Any()) return;

        var equipments = baseEquipments.Select(b => new Equipment(
            b.Name,
            b.VideoUrl,
            b.MuscleGroup,
            b.Id,
            coachId
        ));

        await equipmentRepository.AddRangeAsync(equipments);
    }
}
