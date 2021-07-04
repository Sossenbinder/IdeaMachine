using System;
using Autofac.Extensions.DependencyInjection;
using IdeaMachine.Service.Base.Extensions;
using IdeaMachine.Service.Base.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IdeaMachine.Service.Base
{
	public static class CommonWebHostBuilder
	{
		public static IHost Create<TStartUp>(
			string[] args,
			Action<IHostBuilder>? hostBuilderEnricher = null,
			Action<IWebHostBuilder>? webHostBuilderEnricher = null)
			where TStartUp : class
		{
			var hostBuilder = Host
				.CreateDefaultBuilder(args)
				.UseServiceProviderFactory(new AutofacServiceProviderFactory())
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<TStartUp>();

					webHostBuilderEnricher?.Invoke(webBuilder);
				})
				.ConfigureLogging(x =>
				{
					x.SetMinimumLevel(LogLevel.Warning);
				});

			hostBuilderEnricher?.Invoke(hostBuilder);

			return hostBuilder.Build();
		}

		public static IHost CreateGrpcService<TStartUp>(
			string[] args,
			Action<IHostBuilder>? hostBuilderEnricher = null,
			Action<IWebHostBuilder>? webHostBuilderEnricher = null)
			where TStartUp : class
		{
			var hostBuilder = Host
				.CreateDefaultBuilder(args)
				.UseServiceProviderFactory(new AutofacServiceProviderFactory())
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder
						.UseStartup<TStartUp>()
						.UseConfiguration(ConfigHelper.CreateConfiguration(args))
						.ConfigureGrpc();

					webHostBuilderEnricher?.Invoke(webBuilder);
				})
				.ConfigureLogging(x =>
				{
					x.SetMinimumLevel(LogLevel.Warning);
				});

			hostBuilderEnricher?.Invoke(hostBuilder);

			return hostBuilder.Build();
		}
	}
}