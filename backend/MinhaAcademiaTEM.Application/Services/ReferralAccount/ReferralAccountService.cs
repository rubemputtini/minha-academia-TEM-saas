using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.ReferralAccount;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Application.Services.ReferralAccount;

public class ReferralAccountService(
    ICurrentUserService currentUserService,
    EntityLookup lookup,
    IReferralAccountRepository referralRepository)
    : IReferralAccountService
{
    public async Task<CoachReferralResponse> GetMyCoachReferralAsync()
    {
        var userId = currentUserService.GetUserId();
        var coach = await lookup.GetCoachByUserIdAsync(userId);

        var account = await referralRepository.GetByCoachIdAsync(coach.Id);

        if (account == null)
        {
            account = new Domain.Entities.ReferralAccount(coach.Id);
            await referralRepository.AddAsync(account);
        }

        var referralCode = ReferralCode.FromSlug(coach.Slug);

        return new CoachReferralResponse
        {
            ReferralCode = referralCode,
            CreditsAvailable = account.CreditsAvailable,
            TotalCreditsEarned = account.TotalCreditsEarned,
            UsedCredits = account.GetUsedCredits(),
        };
    }
}