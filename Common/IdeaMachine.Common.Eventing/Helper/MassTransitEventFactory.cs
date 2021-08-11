using IdeaMachine.Common.Eventing.Events;
using IdeaMachine.Common.Eventing.MassTransit.Service.Interface;
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

		public MtEvent<T> CreateDistinct<T>()
			where T : class =>
			new(_massTransitEventingService, new DistinctQueueNameFactory(), _logger);

		public MtEvent<T> CreateDistinct<T>(string queueName)
			where T : class =>
			new(queueName, _massTransitEventingService, new DistinctQueueNameFactory(), _logger);

		public MtEvent<T> CreateRegular<T>()
			where T : class =>
			new(_massTransitEventingService, new SharedQueueNameFactory(), _logger);

		public MtEvent<T> CreateRegular<T>(string queueName)
			where T : class =>
			new(queueName, _massTransitEventingService, new SharedQueueNameFactory(), _logger);
	}
}