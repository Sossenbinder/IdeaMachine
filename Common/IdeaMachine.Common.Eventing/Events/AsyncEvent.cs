using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Extensions.Async;
using IdeaMachine.Common.Eventing.Abstractions.Events;

namespace IdeaMachine.Common.Eventing.Events
{
	public class AsyncEvent : IAsyncEvent
	{
		private readonly List<Func<Task>> _registeredFuncs;

		public AsyncEvent()
		{
			_registeredFuncs = new List<Func<Task>>();
		}

		public async Task<IEnumerable<Exception>> Raise()
		{
			var exceptions = await _registeredFuncs.ParallelAsync(async x =>
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
			_registeredFuncs.Add(() =>
			{
				actionItem();

				return Task.CompletedTask;
			});
		}

		public void Register(Func<Task> actionItem)
		{
			_registeredFuncs.Add(actionItem);
		}

		public void Unregister(Func<Task> actionItem)
		{
			_registeredFuncs.Remove(actionItem);
		}
	}

	public class AsyncEvent<T> : IAsyncEvent<T>
	{
		private readonly List<Func<T, Task>> _registeredFuncs;

		public AsyncEvent()
		{
			_registeredFuncs = new List<Func<T, Task>>();
		}

		public async Task<IEnumerable<Exception>> Raise(T eventArgs)
		{
			var exceptions = await _registeredFuncs.ParallelAsync(async x =>
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
			_registeredFuncs.Add(args =>
			{
				actionItem(args);

				return Task.CompletedTask;
			});
		}

		public void Register(Func<T, Task> actionItem)
		{
			_registeredFuncs.Add(actionItem);
		}

		public void Unregister(Func<T, Task> actionItem)
		{
			_registeredFuncs.Remove(actionItem);
		}
	}
}