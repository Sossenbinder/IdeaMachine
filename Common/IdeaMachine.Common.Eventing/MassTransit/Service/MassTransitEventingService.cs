using System;
using System.Threading.Tasks;
using GreenPipes;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Common.Eventing.MassTransit.Service.Interface;
using MassTransit;
using MassTransit.ConsumeConfigurators;

namespace IdeaMachine.Common.Eventing.MassTransit.Service
{
	public class MassTransitEventingService : IMassTransitEventingService
	{
		private readonly IPublishEndpoint _publishEndpoint;

		private readonly IReceiveEndpointConnector _receiveEndpointConnector;

		public MassTransitEventingService(
			IPublishEndpoint publishEndpoint,
			IReceiveEndpointConnector receiveEndpointConnector)
		{
			_publishEndpoint = publishEndpoint;
			_receiveEndpointConnector = receiveEndpointConnector;
		}

		public Task RaiseEvent<T>(T message)
			where T : class
		{
			return _publishEndpoint.Publish(message);
		}

		public HostReceiveEndpointHandle RegisterForEvent<T>(string queueName, Action<T> handler)
			where T : class
		{
			return RegisterForEvent(queueName, handler.MakeTaskFunc()!);
		}

		public HostReceiveEndpointHandle RegisterForEvent<T>(string queueName, Func<T, Task> handler) where T : class
		{
			return RegisterForEvent<T>(queueName, ctx => handler(ctx.Message));
		}

		public HostReceiveEndpointHandle RegisterInstanceConsumer<TConsumer>(
			string queueName, 
			TConsumer consumer, 
			Action<IReceiveEndpointConfigurator>? customConfigurator = null,
			Action<IInstanceConfigurator<TConsumer>>? instanceConfigurator = null)
			where TConsumer : class, IConsumer
		{
			return RegisterOnQueue(queueName, ep =>
			{
				ep.Instance(consumer, cfg =>
				{
					instanceConfigurator?.Invoke(cfg);

					cfg.UseDelayedRedelivery(retry => retry
						.Intervals(TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(120), TimeSpan.FromMinutes(300)));

					cfg.UseMessageRetry(retry => retry.Exponential(
						3,
						TimeSpan.FromSeconds(2),
						TimeSpan.FromMinutes(5),
						TimeSpan.FromSeconds(10)));
				});

				customConfigurator?.Invoke(ep);
			});
		}

		private HostReceiveEndpointHandle RegisterForEvent<T>(string queueName, Func<ConsumeContext<T>, Task> handler)
			where T : class
		{
			return RegisterOnQueue(queueName, ep => ep.Handler<T>(ctx => handler(ctx)));
		}

		private HostReceiveEndpointHandle RegisterOnQueue(string queueName, Action<IReceiveEndpointConfigurator> registrationCb)
		{
			return _receiveEndpointConnector.ConnectReceiveEndpoint(queueName, (_, ep) => registrationCb(ep));
		}
	}
}