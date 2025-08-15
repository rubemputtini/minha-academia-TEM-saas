using Microsoft.EntityFrameworkCore;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Persistence;

namespace MinhaAcademiaTEM.Infrastructure.Repositories;

public class EquipmentNoteRepository(ApplicationDbContext dbContext)
    : BaseRepository<EquipmentNote>(dbContext), IEquipmentNoteRepository
{
    public async Task<EquipmentNote?> GetByUserIdAsync(Guid userId, Guid coachId) =>
        await dbContext.EquipmentNotes
            .AsNoTracking()
            .FirstOrDefaultAsync(en => en.UserId == userId && en.CoachId == coachId);
}