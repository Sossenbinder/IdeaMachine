using Azure.Identity;
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

			return configBuilder.AddAzureAppConfiguration(options =>
			{
				options.Connect(intermediateConfig["AppConfigConnectionString"])
					.ConfigureKeyVault(kvConf =>
					{
						kvConf.SetCredential(new ClientSecretCredential("ff009f2c-d651-472e-b73e-e0f894c6011e",
							intermediateConfig["KeyVaultClientId"], intermediateConfig["KeyVaultClientSecret"]));
					});
			});
		}
	}
}