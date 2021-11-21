using IdeaMachine.Common.Logging;
using Microsoft.Extensions.Configuration;

namespace IdeaMachine.Service.Base.Utils.ConfigProvider
{
	public class AbstractConfigProvider
	{
		protected IConfigurationBuilder ConfigBuilder = null!;

		private readonly string _environment;

		protected AbstractConfigProvider(string environment)
		{
			_environment = environment;
		}

		public IConfiguration BuildConfig(string[] args, IConfigurationBuilder? configurationBuilder = null)
		{
			var builder = configurationBuilder ?? new ConfigurationBuilder();

			builder
				.AddEnvironmentVariables()
				.AddJsonFile("appsettings.json", true, true)
				.AddJsonFile($"appsettings.{_environment}.json", true, true)
				.AddInMemoryCollection(LoggerConstants.GetConstantsAsInMemoryDict())
				.AddCommandLine(args);

			ConfigBuilder = builder;

			var finalBuilder = AddDeploymentConfig(ConfigBuilder);

			return finalBuilder.Build();
		}

		protected virtual IConfigurationBuilder AddDeploymentConfig(IConfigurationBuilder configBuilder)
		{
			return configBuilder;
		}
	}
}