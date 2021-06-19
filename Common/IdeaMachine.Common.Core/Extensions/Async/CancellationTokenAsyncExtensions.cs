using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace IdeaMachine.Common.Core.Extensions.Async
{
	public static class CancellationTokenAsyncExtensions
	{
		public static CancellationTokenAwaiter GetAwaiter(this CancellationToken ct)
		{
			return new(ct);
		}

		public readonly struct CancellationTokenAwaiter : INotifyCompletion
		{
			private readonly CancellationToken _cancellationToken;

			public CancellationTokenAwaiter(CancellationToken cancellationToken)
			{
				_cancellationToken = cancellationToken;
			}

			public object GetResult()
			{
				if (IsCompleted)
				{
					throw new OperationCanceledException();
				}

				throw new InvalidOperationException("The cancellation token has not yet been cancelled.");
			}

			public bool IsCompleted => _cancellationToken.IsCancellationRequested;

			public void OnCompleted(Action continuation) => _cancellationToken.Register(continuation);
		}
	}
}