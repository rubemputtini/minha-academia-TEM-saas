using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Domain.Interfaces;

public interface IEquipmentRepository : IBaseRepository<Equipment>
{
    Task<Equipment?> GetByIdWithBaseAsync(Guid id);
    Task<IEnumerable<Equipment>> GetAllByCoachIdAsync(Guid coachId);
    Task<IEnumerable<Equipment>> GetActiveByCoachIdAsync(Guid coachId);
}