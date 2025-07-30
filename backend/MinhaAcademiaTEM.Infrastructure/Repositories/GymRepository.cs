using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Persistence;

namespace MinhaAcademiaTEM.Infrastructure.Repositories;

public class GymRepository(ApplicationDbContext dbContext) : IGymRepository
{
    public async Task AddAsync(Gym gym)
    {
        dbContext.Gyms.Add(gym);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Gym gym)
    {
        dbContext.Gyms.Update(gym);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Gym gym)
    {
        dbContext.Gyms.Remove(gym);
        await dbContext.SaveChangesAsync();
    }
}