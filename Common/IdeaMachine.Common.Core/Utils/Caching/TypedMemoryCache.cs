using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace IdeaMachine.Common.Core.Utils.Caching
{
	public class TypedMemoryCache<TKey, TValue>
	{
		private readonly IMemoryCache _cache;

		public TypedMemoryCache(MemoryCacheOptions? memoryCacheOptions = null)
		{
			_cache = new MemoryCache(memoryCacheOptions ?? new MemoryCacheOptions());
		}

		public TValue Get(TKey key)
		{
			return _cache.Get<TValue>(key);
		}

		public TValue Set(TKey key, TValue value, DateTimeOffset expiration)
		{
			return _cache.Set(key, value, expiration);
		}

		public TValue Set(TKey key, TValue value, TimeSpan expiration)
		{
			return _cache.Set(key, value, expiration);
		}

		public TValue Set(TKey key, TValue value)
		{
			return _cache.Set(key, value);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return _cache.TryGetValue(key, out value);
		}

		public void RemoveByKey(TKey key)
		{
			_cache.Remove(key);
		}

		public TValue GetOrCreate(TKey key, Func<ICacheEntry, TValue> factory)
		{
			return _cache.GetOrCreate(key, factory);
		}

		public async ValueTask<TValue> GetOrCreateAsync(TKey key, Func<ICacheEntry, Task<TValue>> factory)
		{
			return await _cache.GetOrCreateAsync(key, factory);
		}
	}
}