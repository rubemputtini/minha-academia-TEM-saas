using Microsoft.Extensions.Caching.Memory;
using MinhaAcademiaTEM.Application.Caching;
using MinhaAcademiaTEM.Application.DTOs.Coaches;
using MinhaAcademiaTEM.Application.DTOs.Users;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Application.Services.Admins;

public class AdminService(
    ICoachRepository coachRepository,
    IUserRepository userRepository,
    IMemoryCache memoryCache)
    : IAdminService
{
    public async Task<(IEnumerable<CoachResponse> Coaches, int TotalCoaches)> GetAllCoachesAsync(
        int page = 1, int pageSize = 10, string? searchTerm = null)
    {
        var isSearch = !string.IsNullOrEmpty(searchTerm);
        var cacheKey = CacheKeys.AllCoaches(page, pageSize);

        if (!isSearch && memoryCache.TryGetValue(cacheKey, out (IEnumerable<CoachResponse> Coaches, int TotalCoaches) cachedCoaches))
            return cachedCoaches;

        var skip = (page - 1) * pageSize;

        var totalCoaches = await coachRepository.CountAsync(searchTerm);
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
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));

            memoryCache.Set(cacheKey, result, cacheOptions);
        }

        return result;
    }

    public async Task<(IEnumerable<UserResponse> Users, int TotalUsers)> GetAllUsersAsync(
        int page = 1, int pageSize = 10, string? searchTerm = null)
    {
        var isSearch = !string.IsNullOrEmpty(searchTerm);
        var cacheKey = CacheKeys.AllUsers(page, pageSize);

        if (!isSearch && memoryCache.TryGetValue(cacheKey, out (IEnumerable<UserResponse> Users, int TotalUsers) cachedUsers))
            return cachedUsers;

        var skip = (page - 1) * pageSize;

        var totalUsers = await userRepository.CountAsync(searchTerm);
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
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));

            memoryCache.Set(cacheKey, result, cacheOptions);
        }

        return result;
    }

    public Task<int> GetTotalCoachesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<int> GetTotalUsersAsync()
    {
        throw new NotImplementedException();
    }

    public Task DeleteCoachAsync(Guid coachId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}