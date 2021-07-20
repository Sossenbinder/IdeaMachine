using System;
using IdeaMachine.Common.Core.Utils.Retry;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IdeaMachine.Common.Eventing.MassTransit
{
	public class MassTransitBusFactory
	{
		private readonly ILogger<MassTransitBusFactory> _logger;

		private readonly IConfiguration _configuration;

		public MassTransitBusFactory(
			ILogger<MassTransitBusFactory> logger,
			IConfiguration configuration)
		{
			_logger = logger;
			_configuration = configuration;
		}

		/// <summary>
		/// Creates a MT Bus with retry logic included (in case the RabbitMQ endpoint starts up later)
		/// </summary>
		public IBusControl CreateBus(
			bool isDevelopmentEnvironment,
			Action<IBusFactoryConfigurator>? configFunc = null)
		{
			return Bus.Factory.CreateUsingRabbitMq(async config =>
			{
				config.Durable = false;
				config.AutoDelete = true;
				config.PurgeOnStartup = true;

				await RetryStrategy.DoRetryExponential(() =>
				{
					config.Host($"rabbitmq://{_configuration["MoonWatch_RabbitMq"]}");
				}, retryCount =>
				{
					_logger.LogInformation($"Retrying RabbitMQ setup for the {retryCount}# time");
				});

				configFunc?.Invoke(config);
			});
		}
	}
}