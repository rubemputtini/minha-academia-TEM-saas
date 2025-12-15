namespace MinhaAcademiaTEM.Application.DTOs.ReferralAccount;

public sealed class CoachReferralResponse
{
    public string ReferralCode { get; init; } = string.Empty;
    public int CreditsAvailable { get; init; }
    public int TotalCreditsEarned { get; init; }
    public int UsedCredits { get; init; }
}