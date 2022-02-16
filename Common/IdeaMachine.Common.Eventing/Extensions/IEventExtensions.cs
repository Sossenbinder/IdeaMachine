using System;
using System.Threading;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Extensions.Async;
using IdeaMachine.Common.Eventing.Abstractions.Events;

namespace IdeaMachine.Common.Eventing.Extensions
{
	// ReSharper disable once InconsistentNaming
	public static class IEventExtensions
	{
		public static Task<bool> Wait(
			this IEvent @event,
			TimeSpan timeout,
			Func<ValueTask>? onTimeout = default)
		{
			var cts = new CancellationTokenSource(timeout);
			return Wait(@event, onTimeout, cts.Token);
		}

		public static async Task<bool> Wait(
			this IEvent @event,
			Func<ValueTask>? onTimeout = default,
			CancellationToken cancellationToken = default)
		{
			var tcs = new TaskCompletionSource();

			async Task RunTimeout()
			{
				await cancellationToken;
			}

			using (@event.Register(() =>
				   {
					   tcs.SetResult();
					   return Task.CompletedTask;
				   }))
			{
				var timeoutTask = RunTimeout();
				var winningTask = await Task.WhenAny(tcs.Task, timeoutTask);

				if (winningTask != timeoutTask)
				{
					return true;
				}

				await (onTimeout?.Invoke() ?? ValueTask.CompletedTask);
				return false;
			}
		}

		public static Task<bool> Wait<TEventArgs>(
			this IEvent<TEventArgs> @event,
			TimeSpan timeout,
			Func<TEventArgs, bool>? eventFilterFunc = default,
			Func<ValueTask>? onTimeout = default)
		{
			var cts = new CancellationTokenSource(timeout);
			return Wait(@event, eventFilterFunc, onTimeout, cts.Token);
		}

		public static async Task<bool> Wait<TEventArgs>(
			this IEvent<TEventArgs> @event,
			Func<TEventArgs, bool>? eventFilterFunc = default,
			Func<ValueTask>? onTimeout = default,
			CancellationToken cancellationToken = default)
		{
			var tcs = new TaskCompletionSource();

			async Task RunTimeout()
			{
				await cancellationToken;
			}

			using (@event.Register(args =>
				   {
					   if (eventFilterFunc is null || eventFilterFunc(args))
					   {
						   tcs.SetResult();
					   }

					   return Task.CompletedTask;
				   }))
			{
				var timeoutTask = RunTimeout();
				var winningTask = await Task.WhenAny(tcs.Task, timeoutTask);

				if (winningTask != timeoutTask)
				{
					return true;
				}

				await (onTimeout?.Invoke() ?? ValueTask.CompletedTask);
				return false;
			}
		}
	}
}