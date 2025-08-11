using Microsoft.EntityFrameworkCore;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Persistence;

namespace MinhaAcademiaTEM.Infrastructure.Repositories;

public class GymRepository(ApplicationDbContext dbContext) : BaseRepository<Gym>(dbContext), IGymRepository
{
    public async Task<Gym?> GetByUserIdAsync(Guid userId) =>
        await dbContext.Gyms.FirstOrDefaultAsync(g => g.UserId == userId);
}