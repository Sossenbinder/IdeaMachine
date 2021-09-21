using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.ConsumeConfigurators;

namespace IdeaMachine.Common.Eventing.MassTransit.Service.Interface
{
	public interface IMassTransitEventingService
	{
		Task RaiseEvent<T>(T message)
			where T : class;

		HostReceiveEndpointHandle RegisterForEvent<T>(string queueName, Action<T> handler)
			where T : class;

		HostReceiveEndpointHandle RegisterForEvent<T>(string queueName, Func<T, Task> handler)
			where T : class;

		HostReceiveEndpointHandle RegisterInstanceConsumer<TConsumer>(
			string queueName,
			TConsumer consumer,
			Action<IReceiveEndpointConfigurator>? customConfigurator = null,
			Action<IInstanceConfigurator<TConsumer>>? instanceConfigurator = null)
			where TConsumer : class, IConsumer;
	}
}