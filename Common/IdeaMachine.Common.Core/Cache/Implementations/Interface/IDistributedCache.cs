using System;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Cache.Implementations.Interface
{
	public interface IDistributedCache<in TKey, TValue> : ILockableCache<TKey, TValue>
	{
		Task<TValue> Get(TKey key);

		Task<TValue?> GetOrDefault(TKey key);

		Task<TValue> GetOrAdd(TKey key, Func<TValue> factory);

		Task<bool> Has(TKey key);

		Task Set(TKey key, TValue value, TimeSpan? slidingExpiration = null);

		Task Delete(TKey key);
	}
}