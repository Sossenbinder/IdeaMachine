using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Cache.Locking.Interface;

namespace IdeaMachine.Common.Core.Cache.Locking.Locks
{
	public abstract class AbstractCacheLock : ICacheLock
	{
		public abstract Task Lock(TimeSpan? expirationTimeSpan = default);

		public abstract ValueTask Release();

		public void Dispose()
		{
			Release().GetAwaiter().GetResult();
		}

		public ValueTask DisposeAsync()
		{
			return Release();
		}
	}
}