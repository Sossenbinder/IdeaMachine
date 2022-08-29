using IdeaMachine.Common.Core.Cache.Locking.Locks;

namespace IdeaMachine.Common.Core.Cache.Locking
{
	public class MemoryCacheLockManager<TKey> : AbstractCacheLockManager<TKey>
		where TKey : notnull
	{
		public MemoryCacheLockManager()
			: base(disposeFunc => new SemaphoreLock(disposeFunc))
		{
		}
	}
}