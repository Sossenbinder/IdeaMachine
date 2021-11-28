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
            // Get our lock first - Then we can check if an item is there at all.
            // If we would check first, then there is no guarantee the item is still
            // there at the point in time we get our turn on the lock
            var @lock = await _cacheLockManager.GetLockLocked(key);
	        var value = Get(key);

	        if (value != null)
	        {
		        return new LockedCacheItem<TValue>(value, @lock);
	        }

	        // In case the item is not there, remove the lock again and return null
	        _cacheLockManager.ReleaseLock(key);
	        throw new KeyNotFoundException($"Item for key {key} not found");
        }

        public async Task<LockedCacheItem<TValue>?> TryGetLocked(TKey key)
		{
			// Get our lock first - Then we can check if an item is there at all.
			// If we would check first, then there is no guarantee the item is still
			// there at the point in time we get our turn on the lock
			var @lock = await _cacheLockManager.GetLockLocked(key);
			var value = Get(key);

			if (value != null)
			{
				return new LockedCacheItem<TValue>(value, @lock);
			}

			// In case the item is not there, remove the lock again and return null
			_cacheLockManager.ReleaseLock(key);
			return null;
		}

        public void Set(TKey key, TValue value)
        {
	        _cache.Set(key, value);
        }
    }
}
