using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Eventing.DataTypes;
using MassTransit;

namespace IdeaMachine.Common.Eventing.MassTransit.Service.Interface
{
	public interface IMassTransitEventingService
	{
		Task RaiseEvent<T>(T message)
			where T : class;

		void RegisterForEvent<T>(string queueName, Action<T> handler, QueueType queueType = QueueType.Regular)
			where T : class;

		void RegisterForEvent<T>(string queueName, Func<T, Task> handler, QueueType queueType = QueueType.Regular)
			where T : class;

		void RegisterConsumer<TConsumer>(string queueName, TConsumer consumer, QueueType queueType = QueueType.Regular)
			where TConsumer : class, IConsumer;
	}
}