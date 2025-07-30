using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Domain.Interfaces;

public interface IGymRepository
{
    Task AddAsync(Gym gym);
    Task UpdateAsync(Gym gym);
    Task DeleteAsync(Gym gym);
}