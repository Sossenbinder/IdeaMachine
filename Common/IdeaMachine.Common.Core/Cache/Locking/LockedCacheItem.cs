using System;
using System.Threading;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Cache.Locking.Interface;
using IdeaMachine.Common.Core.Extensions.Async;

namespace IdeaMachine.Common.Core.Cache.Locking
{
	public class LockedCacheItem<T> : IDisposable, IAsyncDisposable
	{
		public T Value { get; set; }

		private readonly ICacheLock _cacheLock;

		private bool _isDisposed;

		private readonly CancellationTokenSource _cancellationTokenSource;

		private readonly TimeSpan _timeSpan = TimeSpan.FromSeconds(5);

		public LockedCacheItem(
			T value,
			ICacheLock cacheLock)
		{
			_cancellationTokenSource = new CancellationTokenSource();
			_cacheLock = cacheLock;
			Value = value;

			_ = RunAutoRelease().IgnoreTaskCancelledException();
		}

		private async Task RunAutoRelease()
		{
			await Task.Delay(_timeSpan, _cancellationTokenSource.Token);
			await Release();
		}

		public async Task Release()
		{
			if (_isDisposed)
			{
				return;
			}

			_cancellationTokenSource.Cancel();
			_isDisposed = true;
			await _cacheLock.Release();
		}

		public void Dispose() => DisposeAsync().GetAwaiter().GetResult();

		public async ValueTask DisposeAsync()
		{
			await Release();
		}
	}
}