using MinhaAcademiaTEM.Application.Caching;
using MinhaAcademiaTEM.Application.DTOs.Users;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Application.Services.Coaches;

public class CoachService(
    ICurrentUserService currentUserService,
    IAppCacheService cacheService,
    IUserRepository userRepository)
    : ICoachService
{
    public async Task<(IEnumerable<UserResponse> Clients, int TotalClients)> GetAllCoachClientsAsync(
        int page = 1, int pageSize = 10, string? searchTerm = null)
    {
        var coachId = currentUserService.GetUserId();

        var isSearch = !string.IsNullOrEmpty(searchTerm);
        var cacheKey = CacheKeys.CoachClientsPaged(coachId, page, pageSize);

        if (!isSearch && cacheService.TryGetValue(cacheKey, out (IEnumerable<UserResponse> Clients, int TotalClients) cachedClients))
            return cachedClients;

        var skip = (page - 1) * pageSize;

        var totalClients = await userRepository.CountByCoachAsync(coachId, searchTerm);
        var clients = await userRepository.SearchByCoachAsync(coachId, searchTerm, skip, pageSize);

        var responses = clients.Select(u => new UserResponse
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email!,
            CoachId = u.CoachId,
            CoachName = u.Coach!.Name
        });

        var result = (responses, totalClients);

        if (!isSearch)
            cacheService.Set(cacheKey, result);

        return result;
    }

    public async Task DeleteCoachClientAsync(Guid userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        
        if (user == null || user.CoachId != currentUserService.GetUserId())
            throw new NotFoundException("Cliente não encontrado ou não pertence a este treinador.");

        await userRepository.DeleteAsync(user);
    }
}