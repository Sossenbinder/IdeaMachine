using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Cache.Locking;
using Microsoft.Extensions.Caching.Memory;

namespace IdeaMachine.Common.Core.Cache.Implementations.Interface
{
	/// <summary>
	/// A generic cache interface which might be using MemoryCache, Redis etc.
	/// </summary>
	public interface ICache<in TKey, TValue>
    {
	    TValue Get(TKey key);

	    bool TryGetValue(TKey key, out TValue value);

	    TValue GetOrAdd(TKey key, Func<ICacheEntry, TValue> factory);

	    Task<LockedCacheItem<TValue>> GetLocked(TKey key);

	    Task<LockedCacheItem<TValue>?> TryGetLocked(TKey key);

	    void Set(TKey key, TValue value);
    }
}
