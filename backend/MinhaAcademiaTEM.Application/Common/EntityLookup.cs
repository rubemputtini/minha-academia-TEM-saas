using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Application.Common;

public sealed class EntityLookup(IUserRepository users, ICoachRepository coaches, IGymRepository gyms)
{
    public async Task<User> GetUserAsync(Guid userId) =>
        await users.GetByIdAsync(userId) ?? throw new NotFoundException("Usuário não encontrado.");

    public async Task<Coach> GetCoachAsync(Guid coachId) =>
        await coaches.GetByIdAsync(coachId) ?? throw new NotFoundException("Treinador não encontrado.");

    public async Task<Coach> GetCoachByUserIdAsync(Guid userId) =>
        await coaches.GetByUserIdAsync(userId) ?? throw new NotFoundException("Treinador não encontrado.");

    public async Task<Coach> GetCoachByStripeCustomerIdAsync(string customerId) =>
        await coaches.GetByStripeCustomerIdAsync(customerId) ??
        throw new NotFoundException("Treinador não encontrado.");

    public async Task<Gym> GetGymByUserIdAsync(Guid userId) =>
        await gyms.GetByUserIdAsync(userId) ?? throw new NotFoundException("Academia não encontrada.");
}