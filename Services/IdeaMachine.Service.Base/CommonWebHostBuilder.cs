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
		public static IHost Create<TStartup>(
			string[] args,
			Action<IHostBuilder>? hostBuilderEnricher = null,
			Action<IWebHostBuilder>? webHostBuilderEnricher = null)
			where TStartup : class
			=> CreateHost<TStartup>(args, hostBuilderEnricher, webHostBuilderEnricher);


		public static IHost CreateGrpcService<TStartup>(
			string[] args,
			Action<IHostBuilder>? hostBuilderEnricher = null,
			Action<IWebHostBuilder>? webHostBuilderEnricher = null)
			where TStartup : class
			=> CreateHost<TStartup>(args, hostBuilderEnricher, wbe =>
			{
				wbe = wbe.ConfigureGrpc();
				webHostBuilderEnricher?.Invoke(wbe);
			});

		private static IHost CreateHost<TStartup>(
			string[] args,
			Action<IHostBuilder>? hostBuilderEnricher = null,
			Action<IWebHostBuilder>? webHostBuilderEnricher = null)
			where TStartup : class
		{
			var hostBuilder = Host
				.CreateDefaultBuilder(args)
				.UseServiceProviderFactory(new AutofacServiceProviderFactory())
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder = webBuilder
						.UseStartup<TStartup>()
						.UseConfiguration(ConfigHelper.CreateConfiguration(args));
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