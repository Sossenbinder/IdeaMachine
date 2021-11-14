using Microsoft.Extensions.Hosting;

namespace IdeaMachine.ProfilePictureService
{
	public class Program
	{
		public static void Main()
		{
			var host = new HostBuilder()
				.ConfigureFunctionsWorkerDefaults()
				.Build();

			host.Run();
		}
	}
}