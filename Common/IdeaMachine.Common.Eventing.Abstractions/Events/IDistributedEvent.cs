using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Disposable;
using MassTransit;

namespace IdeaMachine.Common.Eventing.Abstractions.Events
{
	public interface IDistributedEvent : IEvent
	{
		Task Raise();

		DisposableAction RegisterForErrors(Func<Fault, Task> faultHandler);
	}

	public interface IDistributedEvent<TEventArgs> : IEvent<TEventArgs>
		where TEventArgs : class
	{
		Task Raise(TEventArgs eventArgs);

		DisposableAction RegisterForErrors(Func<Fault<TEventArgs>, Task> faultHandler);
	}
}