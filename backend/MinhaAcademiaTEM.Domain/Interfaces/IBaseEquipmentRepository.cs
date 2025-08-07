using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Domain.Interfaces;

public interface IBaseEquipmentRepository : IBaseRepository<BaseEquipment>
{
    Task<IEnumerable<BaseEquipment>> GetAllAsync();
}