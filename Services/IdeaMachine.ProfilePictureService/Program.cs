using System;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Hosting;
using IdeaMachine.Common.Logging.Log;
using IdeaMachine.Common.SignalR;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Events;
using IdeaMachine.Modules.Account.Repository.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace IdeaMachine.ProfilePictureService
{
	public class Program
	{
		public static void Main()
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
				.ConfigureServices((ctx, serviceCollection) =>
				{
					serviceCollection.AddDbContext<AccountContext>(options => options.UseSqlServer(ctx.Configuration["DbConnectionString"]));
					serviceCollection.AddSingleton(context => new BlobServiceClient(context.GetRequiredService<IConfiguration>()["BlobStorageConnection"]));

					serviceCollection.AddMassTransit(x =>
					{
						x.UsingRabbitMq((registrationCtx, cfg) =>
						{
							cfg.Durable = false;
							cfg.AutoDelete = true;
							cfg.PurgeOnStartup = true;

							cfg.Host($"rabbitmq://{registrationCtx.GetRequiredService<IConfiguration>()["RabbitMqConnectionString"]}");
							cfg.ReceiveEndpoint(nameof(AccountUpdateProfilePicture), _ => {});
						});
					});
				})
				.ConfigureLogging((ctx, loggingBuilder) =>
				{
					loggingBuilder.AddConsole();
					loggingBuilder.AddSerilog(LogProvider.CreateLogger(ctx.Configuration));
				})
				.Build();

			host.Run();
		}
	}
}