using System;
using System.Threading.Tasks;
using MassTransit;

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

		HostReceiveEndpointHandle RegisterConsumer<TConsumer>(string queueName, TConsumer consumer)
			where TConsumer : class, IConsumer;
	}
}