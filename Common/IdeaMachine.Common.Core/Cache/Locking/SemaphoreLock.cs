using System;
using System.Threading;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Cache.Locking.Interface;

namespace IdeaMachine.Common.Core.Cache.Locking
{
    public class SemaphoreLock : ICacheLock
    {
	    private readonly SemaphoreSlim _semaphore;

		private readonly TimeSpan _timeout = TimeSpan.FromSeconds(5);

	    public SemaphoreLock()
	    {
		    _semaphore = new SemaphoreSlim(1, 1);
	    }

	    public async Task Lock()
	    {
		    await _semaphore.WaitAsync(_timeout);
	    }

	    public ValueTask Release()
	    {
			_semaphore.Release();

			return default;
	    }
    }
}
