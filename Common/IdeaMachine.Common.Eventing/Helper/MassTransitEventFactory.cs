using System;
using IdeaMachine.Common.Eventing.Events;
using IdeaMachine.Common.Eventing.MassTransit.Service.Interface;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using Microsoft.Extensions.Logging;

namespace IdeaMachine.Common.Eventing.Helper
{
	public class MassTransitEventFactory
	{
		private readonly IMassTransitEventingService _massTransitEventingService;

		private readonly ILogger<MassTransitEventFactory> _logger;

		public MassTransitEventFactory(
			IMassTransitEventingService massTransitEventingService,
			ILogger<MassTransitEventFactory> logger)
		{
			_massTransitEventingService = massTransitEventingService;
			_logger = logger;
		}

		public MtEvent<T> CreateDistinct<T>(Action<IReceiveEndpointConfigurator>? customConfigurator = null, Action<IInstanceConfigurator<IConsumer<T>>>? instanceConfigurator = null)
			where T : class =>
			new(_massTransitEventingService, new DistinctQueueNameFactory(), _logger, customConfigurator, instanceConfigurator);

		public MtEvent<T> CreateDistinct<T>(string queueName, Action<IReceiveEndpointConfigurator>? customConfigurator = null, Action<IInstanceConfigurator<IConsumer<T>>>? instanceConfigurator = null)
			where T : class =>
			new(queueName, _massTransitEventingService, new DistinctQueueNameFactory(), _logger, customConfigurator, instanceConfigurator);

		public MtEvent<T> CreateShared<T>(Action<IReceiveEndpointConfigurator>? customConfigurator = null, Action<IInstanceConfigurator<IConsumer<T>>>? instanceConfigurator = null)
			where T : class =>
			new(_massTransitEventingService, new SharedQueueNameFactory(), _logger, customConfigurator);

		public MtEvent<T> CreateShared<T>(string queueName, Action<IReceiveEndpointConfigurator>? customConfigurator = null, Action<IInstanceConfigurator<IConsumer<T>>>? instanceConfigurator = null)
			where T : class =>
			new(queueName, _massTransitEventingService, new SharedQueueNameFactory(), _logger, customConfigurator, instanceConfigurator);
	}
}