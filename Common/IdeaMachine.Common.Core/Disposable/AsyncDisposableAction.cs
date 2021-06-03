using System;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Disposable
{
	public class AsyncDisposableAction : IDisposable, IAsyncDisposable
	{
		private readonly Action _onDispose;

		private readonly Func<ValueTask> _onDisposeAction;

		private bool _isDisposed;

		public AsyncDisposableAction(
			Action onDispose,
			Func<ValueTask> onDisposeAction)
		{
			_onDispose = onDispose;
			_onDisposeAction = onDisposeAction;
		}

		public void Dispose()
		{
			if (_isDisposed)
			{
				return;
			}

			_isDisposed = true;
			_onDispose();
		}

		public ValueTask DisposeAsync()
		{
			if (_isDisposed)
			{
				return default;
			}

			_isDisposed = true;

			return _onDisposeAction();
		}
	}
}