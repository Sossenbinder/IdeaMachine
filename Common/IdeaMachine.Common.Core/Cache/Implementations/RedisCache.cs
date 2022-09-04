using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Cache.Implementations.Interface;
using IdeaMachine.Common.Core.Cache.Locking;
using IdeaMachine.Common.Core.Cache.Locking.Interface;
using IdeaMachine.Common.Core.Utils.Async;
using IdeaMachine.Common.Core.Utils.Serialization;
using StackExchange.Redis;

namespace IdeaMachine.Common.Core.Cache.Implementations
{
	public class RedisCache<TKey, TValue> : IDistributedCache<TKey, TValue>
		where TKey : notnull
	{
		private readonly AsyncLazy<IConnectionMultiplexer> _connectionMultiplexer;

		private readonly ICacheLockManager<TKey> _cacheLockManager;

		private readonly ISerializerDeserializer _serializerDeserializer;

		private static readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(10);

		public RedisCache(
			AsyncLazy<IConnectionMultiplexer> connectionMultiplexer,
			ISerializerDeserializer serializerDeserializer)
		{
			_connectionMultiplexer = connectionMultiplexer;
			_serializerDeserializer = serializerDeserializer;

			_cacheLockManager = new RedisCacheLockManager<TKey>(connectionMultiplexer);
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
			var @lock = await _cacheLockManager.GetLockLocked(key, expirationTimeSpan);
			var value = await Get(key);

			if (value != null)
			{
				return new LockedCacheItem<TValue>(key.ToString() ?? "Unknown", value, @lock);
			}

			// In case the item is not there, remove the lock again and return null
			await @lock.Release();
			return null;
		}

		private async ValueTask<IDatabase> GetDb()
		{
			return (await _connectionMultiplexer).GetDatabase();
		}

		public async Task<TValue> Get(TKey key)
		{
			var rawValue = (await GetDb()).StringGet(key.ToString());

			if (!rawValue.HasValue)
			{
				throw new KeyNotFoundException($"Item for key {key} not found");
			}

			return _serializerDeserializer.Deserialize<TValue>(rawValue!) ?? throw new SerializationException($"Couldn't deserialize item");
		}

		public async Task<TValue?> GetOrDefault(TKey key)
		{
			var rawValue = (await GetDb()).StringGet(key.ToString());

			if (!rawValue.HasValue)
			{
				return default;
			}

			return _serializerDeserializer.Deserialize<TValue>(rawValue!) ?? throw new SerializationException($"Couldn't deserialize item");
		}

		public async Task<TValue> GetOrAdd(TKey key, Func<TValue> factory)
		{
			var rawValue = (await GetDb()).StringGet(key.ToString());

			if (rawValue.HasValue)
			{
				return _serializerDeserializer.Deserialize<TValue>(rawValue!) ??
					   throw new SerializationException($"Couldn't deserialize item");
			}

			var value = factory();

			return value;
		}

		public async Task<bool> Has(TKey key)
		{
			return await (await GetDb()).KeyExistsAsync(key.ToString());
		}

		public async Task Set(TKey key, TValue value, TimeSpan? slidingExpiration = null)
		{
			slidingExpiration ??= _defaultExpiration;
			await (await GetDb()).StringSetAsync(key.ToString(), _serializerDeserializer.Serialize(value), slidingExpiration);
		}

		public async Task Delete(TKey key)
		{
			await (await GetDb()).KeyDeleteAsync(key.ToString());
		}
	}
}