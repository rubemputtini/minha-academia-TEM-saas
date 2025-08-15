using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Domain.Interfaces;

public interface IEquipmentNoteRepository : IBaseRepository<EquipmentNote>
{
    Task<EquipmentNote?> GetByUserIdAsync(Guid userId, Guid coachId);
}