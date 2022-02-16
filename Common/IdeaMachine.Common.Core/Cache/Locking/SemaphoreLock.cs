using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Cache.Locking
{
	public class SemaphoreLock : AbstractCacheLock
	{
		private readonly SemaphoreSlim _semaphore;

		private readonly TimeSpan _timeout = TimeSpan.FromSeconds(5);

		public SemaphoreLock()
		{
			_semaphore = new SemaphoreSlim(1, 1);
		}

		public override async Task Lock()
		{
			await _semaphore.WaitAsync(_timeout);
		}

		public override ValueTask Release()
		{
			_semaphore.Release();

			return default;
		}
	}
}