using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Eventing.Abstractions.Events
{
	public interface IAsyncEvent
	{
		Task<IEnumerable<Exception>> Raise();

		void RaiseFireAndForget();

		void Register(Action actionItem);

		void Unregister(Func<Task> actionItem);
	}

	public interface IAsyncEvent<T>
	{
		Task<IEnumerable<Exception>> Raise(T eventArgs);

		void RaiseFireAndForget(T eventArgs);

		void Register(Action<T> actionItem);

		void Unregister(Func<T, Task> actionItem);
	}
}