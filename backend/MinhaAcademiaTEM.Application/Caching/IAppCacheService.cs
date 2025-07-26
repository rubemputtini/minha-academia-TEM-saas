using Microsoft.Extensions.Caching.Memory;

namespace MinhaAcademiaTEM.Application.Caching;

public interface IAppCacheService
{
    void Set<T>(string key, T value);
    bool TryGetValue<T>(string key, out T? value);
    void Remove(string key);
}