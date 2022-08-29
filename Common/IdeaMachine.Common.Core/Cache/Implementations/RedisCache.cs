using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Cache.Implementations.Interface;
using IdeaMachine.Common.Core.Cache.Locking;
using IdeaMachine.Common.Core.Cache.Locking.Interface;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;

namespace IdeaMachine.Common.Core.Cache.Implementations
{
	public class RedisCache<TKey, TValue> : IDistributedCache<TKey, TValue>
	{
		private readonly IDatabase _database;

		private readonly ICacheLockManager<TKey> _cacheLockManager;

		public RedisCache(IConnectionMultiplexer connectionMultiplexer)
		{
			_database = connectionMultiplexer.GetDatabase();
		}

		public async Task<LockedCacheItem<TValue>> GetLocked(TKey key, TimeSpan? expirationTimeSpan = default)
		{
			var @lock = await TryGetLockedInternal(key, expirationTimeSpan);

			// In case the item is not there, throw
			if (@lock is null)
			{
				throw new KeyNotFoundException($"Item for key {key} not found");
			}

			return @lock;
		}

		public Task<LockedCacheItem<TValue>?> TryGetLocked(TKey key, TimeSpan? expirationTimeSpan = default)
			=> TryGetLockedInternal(key, expirationTimeSpan);

		private async Task<LockedCacheItem<TValue>?> TryGetLockedInternal(TKey key, TimeSpan? expirationTimeSpan = default)
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
			await @lock.Release();
			return null;
		}

		public TValue Get(TKey key)
		{
			throw new NotImplementedException();
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			throw new NotImplementedException();
		}

		public TValue GetOrAdd(TKey key, Func<ICacheEntry, TValue> factory)
		{
			throw new NotImplementedException();
		}

		public ValueTask Set(TKey key, TValue value, TimeSpan? slidingExpiration = null)
		{
			throw new NotImplementedException();
		}

		public ValueTask Delete(TKey key)
		{
			throw new NotImplementedException();
		}
	}
}