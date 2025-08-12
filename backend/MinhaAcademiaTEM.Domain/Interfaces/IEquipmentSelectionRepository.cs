using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Domain.Interfaces;

public interface IEquipmentSelectionRepository
{
    Task<List<EquipmentSelection>> GetByUserAsync(Guid coachId, Guid userId);
    Task SaveAsync(Guid coachId, Guid userId, IEnumerable<EquipmentSelection> selections);
}