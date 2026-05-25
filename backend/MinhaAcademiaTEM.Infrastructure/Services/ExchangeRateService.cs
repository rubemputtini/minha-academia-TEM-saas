using System.Text.Json;
using MinhaAcademiaTEM.Application.Caching;
using MinhaAcademiaTEM.Application.Services.ExchangeRates;

namespace MinhaAcademiaTEM.Infrastructure.Services;

public class ExchangeRateService(IHttpClientFactory httpClientFactory, IAppCacheService cacheService)
    : IExchangeRateService
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<Dictionary<string, decimal>?> GetRatesAsync(string from, string to)
    {
        var cacheKey = CacheKeys.ExchangeRates(from, to);

        if (cacheService.TryGetValue(cacheKey, out Dictionary<string, decimal>? cached))
            return cached;

        var client = httpClientFactory.CreateClient();
        var response = await client.GetAsync($"https://api.frankfurter.dev/v2/rates?base={from}&quotes={to}");

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        var entries = JsonSerializer.Deserialize<List<RateEntry>>(json, JsonOptions);

        if (entries == null || entries.Count == 0)
            return null;

        var rates = entries.ToDictionary(e => e.Quote, e => e.Rate);

        cacheService.Set(cacheKey, rates);

        return rates;
    }

    private sealed class RateEntry
    {
        public string Quote { get; init; } = string.Empty;
        public decimal Rate { get; init; }
    }
}