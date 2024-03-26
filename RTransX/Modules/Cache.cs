namespace RTransX.Modules;

using System.Collections.Concurrent;

public class Cache<TKey, TValue> where TKey: notnull {
    private ConcurrentDictionary<TKey, Task<TValue>> Caches { get; }

    private Func<TKey, Task<TValue>> AsyncFactory { get; }

    public Cache(Func<TKey, Task<TValue>> asyncfactory) {
        Caches = [];
        AsyncFactory = asyncfactory;
    }

    public async Task<TValue> GetValueAsync(TKey key) {
        return await Caches.GetOrAdd(key, AsyncFactory);
    }
}
