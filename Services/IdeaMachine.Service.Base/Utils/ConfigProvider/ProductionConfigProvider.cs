using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
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
			const string? keyVaultEndpoint = "https://ideamachine.vault.azure.net/";

			var intermediateConfig = configBuilder.Build();

			var keyVaultClient = intermediateConfig["KeyvaultClientId"];

			var keyVaultSecret = intermediateConfig["KeyvaultClientSecret"];

			if (keyVaultSecret is null && keyVaultClient is null)
			{
				return configBuilder.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, keyVaultSecret);
			}

			return configBuilder.AddAzureKeyVault(keyVaultEndpoint, new DefaultKeyVaultSecretManager());
		}
	}
}