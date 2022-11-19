using Microsoft.Extensions.Caching.Memory;

namespace DasCleverle.DcsExport.LiveMap.Caching;

public interface ICache
{
    T GetOrCreate<T>(object key, Func<ICacheEntry, T> factory);

    Task<T> GetOrCreateAsync<T>(object key, Func<ICacheEntry, Task<T>> factory);

    bool TryGetValue<T>(object key, out T value);

    void Remove(object key);

    void Set<T>(object key, T value);

    void Clear();
}
