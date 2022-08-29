using System;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Cache.Locking.Interface
{
	/// <summary>
	/// A class managing cache locks in order to ensure cache locks are persistent for everyone
	/// trying to request them.
	/// </summary>
	public interface ICacheLockManager<in TKey>
	{
		Task<ICacheLock> GetLockLocked(TKey key, TimeSpan? expirationTimeSpan = default);

		void ReleaseLock(TKey key);
	}
}