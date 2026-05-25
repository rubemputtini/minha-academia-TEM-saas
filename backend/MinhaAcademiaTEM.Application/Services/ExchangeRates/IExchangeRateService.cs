namespace MinhaAcademiaTEM.Application.Services.ExchangeRates;

public interface IExchangeRateService
{
    Task<Dictionary<string, decimal>?> GetRatesAsync(string from, string to);
}
