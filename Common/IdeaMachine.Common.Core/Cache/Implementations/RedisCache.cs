using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Cache.Implementations.Interface;
using IdeaMachine.Common.Core.Cache.Locking;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;

namespace IdeaMachine.Common.Core.Cache.Implementations
{
	public class RedisCache<TKey, TValue> : IDistributedCache<TKey, TValue>
	{
		private readonly IDatabase _database;

		public RedisCache(IConnectionMultiplexer connectionMultiplexer)
		{
			_database = connectionMultiplexer.GetDatabase();
		}

		public Task<LockedCacheItem<TValue>> GetLocked(TKey key)
		{
			throw new NotImplementedException();
		}

		public Task<LockedCacheItem<TValue>?> TryGetLocked(TKey key)
		{
			throw new NotImplementedException();
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