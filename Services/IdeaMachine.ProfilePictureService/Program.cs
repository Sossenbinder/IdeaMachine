using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Azure.Storage.Blobs;
using IdeaMachine.Common.Database.Extensions;
using IdeaMachine.Common.Eventing.Abstractions.Options;
using Microsoft.Extensions.Hosting;
using IdeaMachine.Common.Logging.Log;
using IdeaMachine.Modules.Account.DI;
using IdeaMachine.Modules.Account.Repository.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

namespace IdeaMachine.ProfilePictureService
{
	public class Program
	{
		public static async Task Main()
		{
			var host = new HostBuilder()
				.ConfigureFunctionsWorkerDefaults()
				.ConfigureAppConfiguration(configBuilder =>
				{
					var intermediateConfig = configBuilder.Build();

					if (intermediateConfig["ASPNETCORE_ENVIRONMENT"] == Environments.Production)
					{
						configBuilder.AddAzureKeyVault("https://ideamachine.vault.azure.net/", intermediateConfig["KeyVaultClientId"], intermediateConfig["KeyVaultClientSecret"]);
					}
				})
				.UseServiceProviderFactory(new AutofacServiceProviderFactory(builder =>
				{
					builder.RegisterModule<InternalAccountModule>();
					builder.Register(context => new BlobServiceClient(context.Resolve<IConfiguration>()["BlobStorageConnection"]))
						.As<BlobServiceClient>()
						.SingleInstance();
				}))
				.ConfigureServices((ctx, serviceCollection) =>
				{
					var configuration = ctx.Configuration;
					serviceCollection.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMqSettings"));

					serviceCollection.AddMassTransit(x =>
					{
						x.UsingRabbitMq((registrationCtx, cfg) =>
						{
							cfg.Durable = false;
							cfg.AutoDelete = true;
							cfg.PurgeOnStartup = true;

							var options = registrationCtx.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
							cfg.Host($"rabbitmq://{options.UserName}:{options.Password}@{options.BrokerAddress}");
							//cfg.Host($"rabbitmq://{options.BrokerAddress}", "/", rabbitMqConfig =>
							//{
							//	rabbitMqConfig.Username(options.UserName);
							//	rabbitMqConfig.Password(options.Password);
							//});
						});
					});
				})
				.ConfigureLogging((ctx, loggingBuilder) =>
				{
					loggingBuilder.AddConsole();
					loggingBuilder.AddSerilog(LogProvider.CreateLogger(ctx.Configuration));
				})
				.Build();

			await host.RunAsync();
		}
	}
}