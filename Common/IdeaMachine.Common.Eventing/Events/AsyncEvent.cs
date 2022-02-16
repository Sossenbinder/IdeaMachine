using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Extensions.Async;
using IdeaMachine.Common.Eventing.Abstractions.Events;

namespace IdeaMachine.Common.Eventing.Events
{
	public class AsyncEvent : EventBase, IAsyncEvent
	{
		public async Task<IEnumerable<Exception>> Raise()
		{
			var exceptions = await Handlers.ParallelAsync(async x =>
			{
				try
				{
					await x();

					return null;
				}
				catch (Exception e)
				{
					return e;
				}
			});

			return exceptions
				.Where(x => x != null)
				.Select(x => x!);
		}

		public void RaiseFireAndForget() => _ = Raise();

		public void Register(Action actionItem)
		{
			Handlers.Add(() =>
			{
				actionItem();

				return Task.CompletedTask;
			});
		}
	}

	public class AsyncEvent<T> : EventBase<T>, IAsyncEvent<T>
	{
		public async Task<IEnumerable<Exception>> Raise(T eventArgs)
		{
			var exceptions = await Handlers.ParallelAsync(async x =>
			{
				try
				{
					await x(eventArgs);

					return null;
				}
				catch (Exception e)
				{
					return e;
				}
			});

			return exceptions
					.Where(x => x != null)
					.Select(x => x!);
		}

		public void RaiseFireAndForget(T eventArgs) => _ = Raise(eventArgs);

		public void Register(Action<T> actionItem)
		{
			Handlers.Add(args =>
			{
				actionItem(args);

				return Task.CompletedTask;
			});
		}
	}
}