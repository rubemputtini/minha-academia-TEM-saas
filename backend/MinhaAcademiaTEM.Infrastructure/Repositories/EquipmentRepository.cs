using Microsoft.EntityFrameworkCore;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Persistence;

namespace MinhaAcademiaTEM.Infrastructure.Repositories;

public class EquipmentRepository(ApplicationDbContext dbContext)
    : BaseRepository<Equipment>(dbContext), IEquipmentRepository
{
    public async Task<IEnumerable<Equipment>> GetAllByCoachIdAsync(Guid coachId) =>
        await dbContext.Equipments
            .AsNoTracking()
            .Where(e => e.CoachId == coachId)
            .OrderBy(e => e.MuscleGroup)
            .ThenBy(e => e.Name)
            .ToListAsync();

    public async Task<IEnumerable<Equipment>> GetActiveByCoachIdAsync(Guid coachId) =>
        await dbContext.Equipments
            .AsNoTracking()
            .Where(e => e.CoachId == coachId && e.IsActive)
            .OrderBy(e => e.MuscleGroup)
            .ThenBy(e => e.Name)
            .ToListAsync();
}