using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Persistence;

namespace MinhaAcademiaTEM.Infrastructure.Repositories;

public class BaseRepository<T>(ApplicationDbContext dbContext) : IBaseRepository<T> where T : BaseEntity
{
    public async Task<T?> GetByIdAsync(Guid id) =>
        await dbContext.Set<T>().FindAsync(id);

    public async Task AddAsync(T entity)
    {
        dbContext.Set<T>().Add(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        dbContext.Set<T>().Update(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        dbContext.Set<T>().Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}