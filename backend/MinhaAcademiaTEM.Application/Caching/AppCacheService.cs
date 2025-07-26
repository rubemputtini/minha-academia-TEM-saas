using Microsoft.Extensions.Caching.Memory;

namespace MinhaAcademiaTEM.Application.Caching;

public class AppCacheService(IMemoryCache memoryCache) : IAppCacheService
{
    public void Set<T>(string key, T value)
    {
        var options = CacheOptionsProvider.ForKey(key);
        memoryCache.Set(key, value, options);
    }

    public bool TryGetValue<T>(string key, out T? value)
    {
        if (memoryCache.TryGetValue(key, out var result) && result is T t)
        {
            value = t;
            return true;
        }

        value = default;
        return false;
    }

    public void Remove(string key)
    {
        memoryCache.Remove(key);
    }
}