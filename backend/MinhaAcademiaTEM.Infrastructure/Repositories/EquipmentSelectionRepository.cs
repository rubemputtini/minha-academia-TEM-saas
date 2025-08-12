using Microsoft.EntityFrameworkCore;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Persistence;

namespace MinhaAcademiaTEM.Infrastructure.Repositories;

public class EquipmentSelectionRepository(ApplicationDbContext dbContext) : IEquipmentSelectionRepository
{
    public async Task<List<EquipmentSelection>> GetByUserAsync(Guid coachId, Guid userId) =>
        await dbContext.EquipmentSelections
            .AsNoTracking()
            .Where(es => es.CoachId == coachId && es.UserId == userId)
            .ToListAsync();

    public async Task SaveAsync(Guid coachId, Guid userId, IEnumerable<EquipmentSelection> selections)
    {
        var existing = await dbContext.EquipmentSelections
            .Where(es => es.CoachId == coachId && es.UserId == userId)
            .ToListAsync();

        dbContext.EquipmentSelections.RemoveRange(existing);

        await dbContext.EquipmentSelections.AddRangeAsync(selections);

        await dbContext.SaveChangesAsync();
    }
}