using IdeaMachine.Common.Logging;
using Microsoft.Extensions.Configuration;

namespace IdeaMachine.Service.Base.Utils.ConfigProvider
{
	public class AbstractConfigProvider
	{
		protected IConfigurationBuilder ConfigBuilder;

		private readonly string _environment;

		protected AbstractConfigProvider(string environment)
		{
			_environment = environment;

			ConfigBuilder = new ConfigurationBuilder();
		}

		public IConfiguration BuildConfig(string[] args)
		{
			ConfigBuilder
				.AddEnvironmentVariables()
				.AddJsonFile("appsettings.json", true, true)
				.AddJsonFile($"appsettings.{_environment}.json", true, true)
				.AddInMemoryCollection(LoggerConstants.GetConstantsAsInMemoryDict())
				.AddCommandLine(args);

			var finalBuilder = AddDeploymentConfig(ConfigBuilder);

			return finalBuilder.Build();
		}

		protected virtual IConfigurationBuilder AddDeploymentConfig(IConfigurationBuilder configBuilder)
		{
			return configBuilder;
		}
	}
}