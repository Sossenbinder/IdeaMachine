using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace IdeaMachine.Service.Base.Utils.ConfigProvider
{
	internal class DevelopmentConfigProvider : AbstractConfigProvider
	{
		public DevelopmentConfigProvider()
			: base(Environments.Development)
		{
		}

		protected override IConfigurationBuilder AddDeploymentConfig(IConfigurationBuilder configBuilder)
		{
			var appAssembly = Assembly.GetEntryAssembly();
			return appAssembly != null ? ConfigBuilder.AddUserSecrets(appAssembly) : configBuilder;
		}
	}
}