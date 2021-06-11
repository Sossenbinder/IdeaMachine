using Microsoft.Extensions.Configuration;

namespace IdeaMachine.Modules.Email.Utils
{
	public static class EnvironmentLinkGenerator
	{
		private const string FallbackLink = "https://localhost:1457";

		public static string GetDomainLink(IConfiguration configuration)
		{
			var link = configuration["DeploymentLink"];

			return link ?? FallbackLink;
		}
	}
}