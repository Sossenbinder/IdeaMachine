using System.Threading.Tasks;
using IdeaMachine.Common.Core.Cache.Locking.Interface;

namespace IdeaMachine.Common.Core.Cache.Locking
{
	public abstract class AbstractCacheLock : ICacheLock
	{
		public abstract Task Lock();

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