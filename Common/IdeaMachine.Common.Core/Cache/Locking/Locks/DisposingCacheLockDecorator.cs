using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Cache.Locking.Interface;

namespace IdeaMachine.Common.Core.Cache.Locking.Locks
{
	public class DisposingCacheLockDecorator : AbstractCacheLock
	{
		private readonly ICacheLock _actualLock;

		private readonly Action _disposalAction;

		public DisposingCacheLockDecorator(
			ICacheLock actualLock,
			Action disposalAction)
		{
			_actualLock = actualLock;
			_disposalAction = disposalAction;
		}

		public override Task Lock(TimeSpan? expirationTimeSpan = default)
		{
			return _actualLock.Lock(expirationTimeSpan);
		}

		public override async ValueTask Release()
		{
			try
			{
				await _actualLock.Release();
			}
			finally
			{
				_disposalAction();
			}
		}
	}
}