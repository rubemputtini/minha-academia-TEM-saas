using MinhaAcademiaTEM.Application.Caching;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.Coaches;
using MinhaAcademiaTEM.Application.DTOs.Users;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Application.Services.Admins;

public class AdminService(
    EntityLookup lookup,
    ICoachRepository coachRepository,
    IUserRepository userRepository,
    IAppCacheService cacheService)
    : IAdminService
{
    public async Task<(IEnumerable<CoachResponse> Coaches, int TotalCoaches)> GetAllCoachesAsync(
        int page = 1, int pageSize = 10, string? searchTerm = null)
    {
        var isSearch = !string.IsNullOrWhiteSpace(searchTerm);
        var totalCoaches = await coachRepository.CountAsync(searchTerm);

        var cacheKey = CacheKeys.AllCoaches(page, pageSize, totalCoaches);

        if (!isSearch && cacheService.TryGetValue(cacheKey,
                out (IEnumerable<CoachResponse> Coaches, int TotalCoaches) cachedCoaches))
            return cachedCoaches;

        var skip = (page - 1) * pageSize;

        var coaches = await coachRepository.SearchAsync(searchTerm, skip, pageSize);

        var coachIds = coaches.Select(c => c.Id).ToList();
        var clientsCountByCoachId = await userRepository.GetClientsCountForCoachesAsync(coachIds);

        var responses = coaches.Select(c => new CoachResponse
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            PhoneNumber = c.User!.PhoneNumber!,
            IsActive = c.IsActive,
            SubscriptionStatus = c.SubscriptionStatus.ToString(),
            SubscriptionPlan = c.SubscriptionPlan.ToString(),
            SubscriptionEndAt = c.SubscriptionEndAt,
            ClientsCount = clientsCountByCoachId.GetValueOrDefault(c.Id, 0),
        });

        var result = (responses, totalCoaches);

        if (!isSearch)
            cacheService.Set(cacheKey, result);

        return result;
    }

    public async Task<(IEnumerable<UserResponse> Users, int TotalUsers)> GetAllUsersAsync(
        int page = 1, int pageSize = 10, string? searchTerm = null)
    {
        var isSearch = !string.IsNullOrEmpty(searchTerm);
        var totalUsers = await userRepository.CountAsync(searchTerm);

        var cacheKey = CacheKeys.AllUsers(page, pageSize, totalUsers);

        if (!isSearch &&
            cacheService.TryGetValue(cacheKey, out (IEnumerable<UserResponse> Users, int TotalUsers) cachedUsers))
            return cachedUsers;

        var skip = (page - 1) * pageSize;

        var users = await userRepository.SearchAsync(searchTerm, skip, pageSize);

        var responses = users.Select(u => new UserResponse
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email!,
            CoachId = u.CoachId,
            CoachName = u.Coach?.Name,
        });

        var result = (responses, totalUsers);

        if (!isSearch)
            cacheService.Set(cacheKey, result);

        return result;
    }

    public async Task<int> GetTotalCoachesAsync() =>
        await coachRepository.GetTotalCoachesAsync();

    public async Task<int> GetTotalUsersAsync() =>
        await userRepository.GetTotalUsersAsync();

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await lookup.GetUserAsync(userId);

        var coach = await coachRepository.GetByIdAsync(userId);

        if (coach != null)
            await coachRepository.DeleteAsync(coach);

        await userRepository.DeleteAsync(user);
    }
}