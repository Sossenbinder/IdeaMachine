using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Cache.Implementations.Interface;
using IdeaMachine.Common.Core.Cache.Locking;
using IdeaMachine.Common.Core.Cache.Locking.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace IdeaMachine.Common.Core.Cache.Implementations
{
    public class MemoryCache<TKey, TValue> : ICache<TKey, TValue> 
	    where TKey : notnull
    {
	    private readonly IMemoryCache _cache;

	    private readonly ICacheLockManager<TKey> _cacheLockManager;

        public MemoryCache()
	    {
		    _cache = new MemoryCache(new MemoryCacheOptions());
		    _cacheLockManager = new MemoryCacheLockManager<TKey>();
	    }

	    public TValue Get(TKey key)
        {
			return _cache.Get<TValue>(key) ?? throw new KeyNotFoundException($"Item for key {key} not found");
		}

	    public bool TryGetValue(TKey key, out TValue value)
	    {
		    return _cache.TryGetValue(key, out value);
	    }

	    public TValue GetOrAdd(TKey key, Func<ICacheEntry, TValue> factory)
        {
	        return _cache.GetOrCreate(key, factory);
        }

        public async Task<LockedCacheItem<TValue>> GetLocked(TKey key)
        {
	        var @lock = await TryGetLockedInternal(key);

			// In case the item is not there, throw
			if (@lock is null)
			{
				throw new KeyNotFoundException($"Item for key {key} not found");
			}

			return @lock;
        }

        public Task<LockedCacheItem<TValue>?> TryGetLocked(TKey key)
	        => TryGetLockedInternal(key);

        private async Task<LockedCacheItem<TValue>?> TryGetLockedInternal(TKey key)
		{
			// Get our lock first - Then we can check if an item is there at all.
			// If we would check first, then there is no guarantee the item is still
			// there at the point in time we get our turn on the lock
			var @lock = await _cacheLockManager.GetLockLocked(key);
			var value = Get(key);

			if (value != null)
			{
				return new LockedCacheItem<TValue>(key.ToString() ?? "Unknown", value, @lock);
			}

			// In case the item is not there, remove the lock again and return null
			await using (@lock)
			{
				return null;
			}
		}

        public ValueTask Set(TKey key, TValue value)
        {
	        _cache.Set(key, value);
	        return default;
        }

        public ValueTask Delete(TKey key)
        {
	        _cache.Remove(key);
	        return default;
        }
    }
}
