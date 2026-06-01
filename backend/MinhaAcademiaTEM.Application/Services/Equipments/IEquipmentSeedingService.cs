namespace MinhaAcademiaTEM.Application.Services.Equipments;

public interface IEquipmentSeedingService
{
    Task SeedForCoachAsync(Guid coachId);
}
