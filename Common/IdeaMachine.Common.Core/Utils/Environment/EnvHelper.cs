using Microsoft.Extensions.Hosting;

namespace IdeaMachine.Common.Core.Utils.Environment
{
	public static class EnvHelper
	{
		public static string GetDeployment() => System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environments.Production;
	}
}