using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace DasCleverle.DcsExport.LiveMap.Caching;

public class MemoryCache : ICache, IDisposable
{
    private readonly IMemoryCache _cache;
    private CancellationTokenSource _cts;

    public MemoryCache(IMemoryCache cache)
    {
        _cache = cache;
        _cts = new();
    }

    public T GetOrCreate<T>(object key, Func<ICacheEntry, T> factory)
    {
        return _cache.GetOrCreate<T>(key, (entry) =>
        {
            ConfigureEntry(entry);
            return factory(entry);
        });
    }

    public async Task<T> GetOrCreateAsync<T>(object key, Func<ICacheEntry, Task<T>> factory)
    {
        return await _cache.GetOrCreateAsync<T>(key, async (entry) =>
        {
            ConfigureEntry(entry);
            return await factory(entry);
        });
    }

    public bool TryGetValue<T>(object key, out T value)
    {
        return _cache.TryGetValue<T>(key, out value);
    }

    public void Remove(object key)
    {
        _cache.Remove(key);
    }

    public void Set<T>(object key, T value)
    {
        var entry = _cache.CreateEntry(key);

        ConfigureEntry(entry);
        entry.Value = value;
    }

    public void Clear()
    {
        _cts.Cancel();
        _cts.Dispose();
        _cts = new();
    }

    public void Dispose()
    {
        _cts.Dispose();
    }

    private void ConfigureEntry(ICacheEntry entry)
    {
        entry.AddExpirationToken(new CancellationChangeToken(_cts.Token));
    }
}
