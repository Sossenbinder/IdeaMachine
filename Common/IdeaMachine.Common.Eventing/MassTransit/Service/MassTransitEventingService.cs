using System;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using GreenPipes;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Common.Eventing.DataTypes;
using IdeaMachine.Common.Eventing.MassTransit.Service.Interface;
using MassTransit;

namespace IdeaMachine.Common.Eventing.MassTransit.Service
{
	public class MassTransitEventingService : IStartable, IDisposable, IMassTransitEventingService
	{
		private readonly IBusControl _busControl;

		public MassTransitEventingService(IBusControl busControl)
		{
			_busControl = busControl;
		}

		public void Start()
		{
			_busControl.Start();
		}

		public void Dispose()
		{
			_busControl.Stop();
		}

		public Task RaiseEvent<T>(T message)
			where T : class
		{
			return _busControl.Publish(message);
		}

		private void RegisterForEvent<T>(string queueName, Func<ConsumeContext<T>, Task> handler, QueueType queueType = QueueType.Regular)
			where T : class
		{
			RegisterOnQueue(queueName, ep => ep.Handler<T>(ctx => handler(ctx)), queueType);
		}

		public void RegisterForEvent<T>(string queueName, Action<T> handler, QueueType queueType = QueueType.Regular)
			where T : class
		{
			RegisterForEvent<T>(queueName, handler.MakeTaskCompatible()!, queueType);
		}

		public void RegisterForEvent<T>(string queueName, Func<T, Task> handler, QueueType queueType = QueueType.Regular) where T : class
		{
			RegisterForEvent<T>(queueName, ctx => handler(ctx.Message), queueType);
		}

		public void RegisterConsumer<TConsumer>(string queueName, TConsumer consumer, QueueType queueType = QueueType.Regular)
			where TConsumer : class, IConsumer
		{
			RegisterOnQueue(queueName, ep => ep.Instance(consumer), queueType);
		}

		private void RegisterOnQueue(string queueName, Action<IReceiveEndpointConfigurator> registrationCb, QueueType queueType = QueueType.Regular)
		{
			queueName = $"{queueName}_{Dns.GetHostName()}";

			if (queueType == QueueType.Error)
			{
				queueName = $"{queueName}_error";
			}

			_busControl.ConnectReceiveEndpoint(queueName, ep =>
			{
				ep.UseMessageRetry(retry => retry.Exponential(
					10,
					TimeSpan.FromSeconds(2),
					TimeSpan.FromMinutes(5),
					TimeSpan.FromSeconds(10)));

				registrationCb(ep);
			});
		}
	}
}