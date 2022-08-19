using System;
using System.Threading;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Cache.Locking.Interface;
using IdeaMachine.Common.Core.Extensions.Async;
using Serilog;

namespace IdeaMachine.Common.Core.Cache.Locking
{
	/// <summary>
	/// Wraps a cache item with a lock. Takes care of auto-releasing the lock after X seconds as well
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class LockedCacheItem<T> : IDisposable, IAsyncDisposable
	{
		private readonly T _value;
		public T Value => _isDisposed ? throw new InvalidOperationException("Object is already disposed") : _value;

		private readonly string _keyValue;

		private readonly ICacheLock _cacheLock;

		private bool _isDisposed;

		private readonly CancellationTokenSource _cancellationTokenSource;

		private readonly TimeSpan _timeSpan = TimeSpan.FromSeconds(5);

		public LockedCacheItem(
			string keyValue,
			T value,
			ICacheLock cacheLock)
		{
			_cancellationTokenSource = new CancellationTokenSource();
			_keyValue = keyValue;
			_cacheLock = cacheLock;
			_value = value;

			_ = RunAutoRelease().IgnoreTaskCancelledException();
		}

		private async Task RunAutoRelease()
		{
			await Task.Delay(_timeSpan, _cancellationTokenSource.Token);

			if (_isDisposed)
			{
				return;
			}

			Log.Information("Auto-releasing cache item with key {key}", _keyValue);
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