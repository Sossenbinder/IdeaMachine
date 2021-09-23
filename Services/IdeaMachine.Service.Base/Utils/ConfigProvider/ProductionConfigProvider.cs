using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace IdeaMachine.Service.Base.Utils.ConfigProvider
{
	internal class ProductionConfigProvider : AbstractConfigProvider
	{
		public ProductionConfigProvider()
			: base(Environments.Production)
		{
		}

		protected override IConfigurationBuilder AddDeploymentConfig(IConfigurationBuilder configBuilder)
		{
			var intermediateConfig = configBuilder.Build();

			configBuilder = configBuilder.AddAzureKeyVault("https://ideamachine.vault.azure.net/", intermediateConfig["KeyVaultClientId"], intermediateConfig["KeyVaultClientSecret"]);

			intermediateConfig = configBuilder.Build();

			return configBuilder.AddAzureAppConfiguration(intermediateConfig["AppConfigConnectionString"]);
		}
	}
}