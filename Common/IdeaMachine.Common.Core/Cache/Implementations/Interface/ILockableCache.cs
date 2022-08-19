using IdeaMachine.Common.Core.Cache.Locking;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Cache.Implementations.Interface
{
	public interface ILockableCache<in TKey, TValue>
	{
		Task<LockedCacheItem<TValue>> GetLocked(TKey key);

		Task<LockedCacheItem<TValue>?> TryGetLocked(TKey key);
	}
}