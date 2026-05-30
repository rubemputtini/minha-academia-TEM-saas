using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;

namespace MinhaAcademiaTEM.Application.Caching;

public class AppCacheService(IMemoryCache memoryCache) : IAppCacheService
{
    private readonly ConcurrentDictionary<string, byte> _keys = new();

    public void Set<T>(string key, T value)
    {
        var options = CacheOptionsProvider.ForKey(key);
        memoryCache.Set(key, value, options);
        _keys[key] = 0;
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
        _keys.TryRemove(key, out _);
    }

    public void RemoveMultiple(params string[] keys)
    {
        foreach (var key in keys)
            Remove(key);
    }

    public void RemoveByPrefix(string prefix)
    {
        foreach (var key in _keys.Keys.Where(k => k.StartsWith(prefix)).ToList())
            Remove(key);
    }
}
