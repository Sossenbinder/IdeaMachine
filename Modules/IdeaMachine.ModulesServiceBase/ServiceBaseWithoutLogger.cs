using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Disposable;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Common.Eventing.Abstractions.Events;
using MassTransit;

namespace IdeaMachine.ModulesServiceBase
{
	/// <summary>
	/// Handles event registrations with the respective dispose backup. Imagine a service registering on an event
	/// and the service at some point being released. We need to handle removal of the registration in this case.
	/// This is what this class is useful for.
	/// </summary>
	public class ServiceBaseWithoutLogger : Disposable
	{
		protected void RegisterEventHandler<T>(
			IEvent<T> @event,
			Action<T> handler)
			=> RegisterEventHandler(@event, handler.MakeTaskFunc()!);

		protected void RegisterEventHandler<T>(
			IEvent<T> @event,
			Func<T, Task> handler)
		{
			var disposeAction = @event.Register(handler);

			RegisterDisposable(disposeAction);
		}

		protected void RegisterFaultHandler<T>(
			IDistributedEvent<T> @event,
			Action<Fault<T>> handler)
			where T : class
			=> RegisterFaultHandler(@event, handler.MakeTaskFunc()!);

		protected void RegisterFaultHandler<T>(
			IDistributedEvent<T> @event,
			Func<Fault<T>, Task> handler)
			where T : class
		{
			var disposeAction = @event.RegisterForErrors(handler);

			RegisterDisposable(disposeAction);
		}
	}
}