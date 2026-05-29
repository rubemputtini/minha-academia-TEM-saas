using MinhaAcademiaTEM.Application.Caching;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.Admin;
using MinhaAcademiaTEM.Application.DTOs.Coaches;
using MinhaAcademiaTEM.Application.DTOs.Users;
using MinhaAcademiaTEM.Domain.Constants;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Application.Services.Admins;

public class AdminService(
    EntityLookup lookup,
    ICoachRepository coachRepository,
    IUserRepository userRepository,
    IReferralAccountRepository referralAccountRepository,
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
        var referralCountsByCoachId = await referralAccountRepository.GetReferralCountsForCoachesAsync(coachIds);

        var responses = coaches.Select(c => new CoachResponse
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            PhoneNumber = c.User!.PhoneNumber!,
            SubscriptionStatus = c.SubscriptionStatus.ToString(),
            SubscriptionPlan = c.SubscriptionPlan.ToString(),
            SubscriptionEndAt = c.SubscriptionEndAt,
            ClientsCount = clientsCountByCoachId.GetValueOrDefault(c.Id, 0),
            ReferralsCount = referralCountsByCoachId.GetValueOrDefault(c.Id, 0),
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
            IsActive = u.IsActive,
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

    public async Task<AdminStatsResponse> GetStatsAsync()
    {
        if (cacheService.TryGetValue(CacheKeys.AdminStats, out AdminStatsResponse? cached) && cached is not null)
            return cached;

        var statusCounts = await coachRepository.GetCountsByStatusAsync();
        var totalUsers = await userRepository.GetTotalUsersAsync();
        var newCoachesThisMonth = await coachRepository.CountNewThisMonthAsync();
        var newActiveSubscriptionsThisMonth = await coachRepository.CountNewActiveSubscriptionsThisMonthAsync();
        var canceledSubscriptionsThisMonth = await coachRepository.CountCanceledSubscriptionsThisMonthAsync();
        var coachesWithoutClients = await coachRepository.CountWithoutClientsAsync(daysThreshold: 30);
        var planCounts = await coachRepository.GetActiveCountsByPlanAsync();

        var activeCoaches = statusCounts.GetValueOrDefault(SubscriptionStatus.Active);
        var trialCoaches = statusCounts.GetValueOrDefault(SubscriptionStatus.Trial);
        var pastDueCoaches = statusCounts.GetValueOrDefault(SubscriptionStatus.PastDue);
        var canceledCoaches = statusCounts.GetValueOrDefault(SubscriptionStatus.Canceled);
        var totalCoaches = statusCounts.Values.Sum();

        var basicCoaches = planCounts.GetValueOrDefault(SubscriptionPlan.Basic, 0);
        var unlimitedCoaches = planCounts.GetValueOrDefault(SubscriptionPlan.Unlimited, 0);

        var result = new AdminStatsResponse
        {
            TotalCoaches = totalCoaches,
            TotalUsers = totalUsers,
            ActiveCoaches = activeCoaches,
            TrialCoaches = trialCoaches,
            PastDueCoaches = pastDueCoaches,
            CanceledCoaches = canceledCoaches,
            NewCoachesThisMonth = newCoachesThisMonth,
            NewActiveSubscriptionsThisMonth = newActiveSubscriptionsThisMonth,
            CanceledSubscriptionsThisMonth = canceledSubscriptionsThisMonth,
            CoachesWithoutClients = coachesWithoutClients,
            BasicCoaches = basicCoaches,
            UnlimitedCoaches = unlimitedCoaches,
            EstimatedMonthlyRevenueBrl = basicCoaches * SubscriptionPlanPrices.BasicBrl +
                                         unlimitedCoaches * SubscriptionPlanPrices.UnlimitedBrl,
        };

        cacheService.Set(CacheKeys.AdminStats, result);

        return result;
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await lookup.GetUserAsync(userId);

        var coach = await coachRepository.GetByIdAsync(userId);

        if (coach != null)
            await coachRepository.DeleteAsync(coach);

        await userRepository.DeleteAsync(user);
        cacheService.Remove(CacheKeys.AdminStats);
    }

    public async Task<UpdateCoachSubscriptionResponse> UpdateCoachSubscriptionAsync(
        Guid coachId, UpdateCoachSubscriptionRequest request)
    {
        var coach = await lookup.GetCoachAsync(coachId);

        if (!string.IsNullOrWhiteSpace(coach.StripeCustomerId) ||
            !string.IsNullOrWhiteSpace(coach.StripeSubscriptionId))
            throw new ValidationException("O treinador já possui dados de compra no Stripe.");

        coach.SetSubscription(request.SubscriptionPlan, request.SubscriptionStatus, request.SubscriptionEndAt);

        await coachRepository.UpdateAsync(coach);
        cacheService.Remove(CacheKeys.AdminStats);

        var response = new UpdateCoachSubscriptionResponse
        {
            CoachId = coach.Id,
            SubscriptionStatus = coach.SubscriptionStatus,
            SubscriptionPlan = coach.SubscriptionPlan,
            SubscriptionEndAt = coach.SubscriptionEndAt,
        };

        return response;
    }
}