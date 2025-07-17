using Microsoft.EntityFrameworkCore;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Persistence;

namespace MinhaAcademiaTEM.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id) =>
        await dbContext.Users.FindAsync(id);

    public async Task<User?> GetByEmailAsync(string email) =>
        await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<List<User>> GetAllAsync() =>
        await dbContext.Users.ToListAsync();

    public async Task<List<User>> GetAllByCoachIdAsync(Guid coachId) =>
        await dbContext.Users.Where(u => u.CoachId == coachId).ToListAsync();

    public async Task AddAsync(User user) =>
        await dbContext.Users.AddAsync(user);

    public Task UpdateAsync(User user)
    {
        dbContext.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(User user)
    {
        dbContext.Users.Remove(user);
        return Task.CompletedTask;
    }
}