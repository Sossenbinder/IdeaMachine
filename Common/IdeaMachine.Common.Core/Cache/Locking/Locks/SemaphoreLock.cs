using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Cache.Locking.Locks
{
	public class SemaphoreLock : AbstractCacheLock
	{
		private readonly SemaphoreSlim _semaphore;

		private readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(5);

		public SemaphoreLock()
		{
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
			return default;
		}
	}
}