using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Cache.Locking.Locks
{
	public class SemaphoreLock : AbstractCacheLock
	{
		private readonly Action _disposeFunc;

		private readonly SemaphoreSlim _semaphore;

		private readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(5);

		public SemaphoreLock(Action disposeFunc)
		{
			_disposeFunc = disposeFunc;
			_semaphore = new SemaphoreSlim(0, 1);
		}

		public override async Task Lock(TimeSpan? expirationTimeSpan = default)
		{
			expirationTimeSpan ??= _defaultTimeout;
			await _semaphore.WaitAsync(expirationTimeSpan.Value);
		}

		public override ValueTask Release()
		{
			_semaphore.Release();
			_disposeFunc();
			return default;
		}
	}
}