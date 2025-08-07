using Microsoft.EntityFrameworkCore;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Persistence;

namespace MinhaAcademiaTEM.Infrastructure.Repositories;

public class BaseEquipmentRepository(ApplicationDbContext dbContext)
    : BaseRepository<BaseEquipment>(dbContext), IBaseEquipmentRepository
{
    public async Task<IEnumerable<BaseEquipment>> GetAllAsync() =>
        await dbContext.BaseEquipments
            .AsNoTracking()
            .ToListAsync();
}