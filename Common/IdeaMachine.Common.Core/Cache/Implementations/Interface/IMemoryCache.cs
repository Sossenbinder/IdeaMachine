using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace IdeaMachine.Common.Core.Cache.Implementations.Interface
{
	public interface IMemoryCache<in TKey, TValue> : ILockableCache<TKey, TValue>
	{
		TValue Get(TKey key);

		bool TryGetValue(TKey key, out TValue value);

		TValue GetOrAdd(TKey key, Func<ICacheEntry, TValue> factory);

		ValueTask Set(TKey key, TValue value, TimeSpan? slidingExpiration = null);

		ValueTask Delete(TKey key);
	}
}