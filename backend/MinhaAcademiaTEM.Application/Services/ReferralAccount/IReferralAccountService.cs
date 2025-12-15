using MinhaAcademiaTEM.Application.DTOs.ReferralAccount;

namespace MinhaAcademiaTEM.Application.Services.ReferralAccount;

public interface IReferralAccountService
{
    Task<CoachReferralResponse> GetMyCoachReferralAsync();
}