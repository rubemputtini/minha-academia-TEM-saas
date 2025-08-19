namespace MinhaAcademiaTEM.Application.Services.Billing;

public interface IBillingPortalService
{
    Task<string> CreateCustomerPortalSessionAsync();
}