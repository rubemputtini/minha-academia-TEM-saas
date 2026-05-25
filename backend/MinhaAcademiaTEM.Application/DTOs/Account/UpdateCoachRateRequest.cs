namespace MinhaAcademiaTEM.Application.DTOs.Account;

public sealed class UpdateCoachRateRequest
{
    public decimal MonthlyRate { get; init; }
    public string Currency { get; init; } = string.Empty;
}
