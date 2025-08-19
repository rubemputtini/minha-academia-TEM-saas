using MinhaAcademiaTEM.Application.DTOs.Billing;

namespace MinhaAcademiaTEM.Application.Services.Billing;

public interface ICheckoutSessionReader
{
    Task<CoachPreFillResponse> GetPrefillAsync(string sessionId);
}