using System;
using System.Collections.Generic;
using IdeaMachine.Common.Core.Utils.Environment;
using IdeaMachine.Service.Base.Utils.ConfigProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace IdeaMachine.Service.Base.Utils
{
	public class ConfigHelper
	{
		private static readonly Dictionary<string, AbstractConfigProvider> ConfigProviders;

		static ConfigHelper()
		{
			ConfigProviders = new Dictionary<string, AbstractConfigProvider>()
			{
				{ Environments.Development, new DevelopmentConfigProvider() },
				{ Environments.Staging, new ProductionConfigProvider() },
				{ Environments.Production, new ProductionConfigProvider() },
			};
		}

		public static IConfiguration CreateConfiguration(string[] args, IConfigurationBuilder? configurationBuilder = default)
		{
			var environment = EnvHelper.GetDeployment();

			Console.WriteLine($"Building config for environment {environment}");

			return ConfigProviders[environment ?? throw new InvalidOperationException()].BuildConfig(args, configurationBuilder);
		}
	}
}