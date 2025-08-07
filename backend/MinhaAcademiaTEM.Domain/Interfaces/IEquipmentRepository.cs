using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Domain.Interfaces;

public interface IEquipmentRepository : IBaseRepository<Equipment>
{
    Task<IEnumerable<Equipment>> GetAllByCoachIdAsync(Guid coachId);
    Task<IEnumerable<Equipment>> GetActiveByCoachIdAsync(Guid coachId);
}