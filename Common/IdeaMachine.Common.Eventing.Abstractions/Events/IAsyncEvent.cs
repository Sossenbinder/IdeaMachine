using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Eventing.Abstractions.Events
{
	public interface IAsyncEvent : IEvent
	{
		Task<IEnumerable<Exception>> Raise();

		void RaiseFireAndForget();
	}

	public interface IAsyncEvent<T> : IEvent<T>
	{
		Task<IEnumerable<Exception>> Raise(T eventArgs);

		void RaiseFireAndForget(T eventArgs);
	}
}