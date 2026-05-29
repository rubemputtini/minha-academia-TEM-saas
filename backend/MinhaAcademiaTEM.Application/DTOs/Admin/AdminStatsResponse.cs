namespace MinhaAcademiaTEM.Application.DTOs.Admin;

public class AdminStatsResponse
{
    public int TotalCoaches { get; init; }
    public int ActiveCoaches { get; init; }
    public int TrialCoaches { get; init; }
    public int PastDueCoaches { get; init; }
    public int CanceledCoaches { get; init; }
    public int TotalUsers { get; init; }
    public int NewCoachesThisMonth { get; init; }
    public int NewActiveSubscriptionsThisMonth { get; init; }
    public int CanceledSubscriptionsThisMonth { get; init; }
    public int CoachesWithoutClients { get; init; }
    public int BasicCoaches { get; init; }
    public int UnlimitedCoaches { get; init; }
    public decimal EstimatedMonthlyRevenueBrl { get; init; }
}
