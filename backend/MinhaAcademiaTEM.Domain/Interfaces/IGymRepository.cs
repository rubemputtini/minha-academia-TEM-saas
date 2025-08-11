using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Domain.Interfaces;

public interface IGymRepository : IBaseRepository<Gym>
{
    Task<Gym?> GetByUserIdAsync(Guid userId);
}