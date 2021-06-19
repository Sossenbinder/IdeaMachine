using System.Threading.Tasks;
using IdeaMachine.Service.Base;
using Microsoft.Extensions.Hosting;

namespace IdeaMachine.AccountService
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var host = CommonWebHostBuilder.CreateGrpcService<Startup>(args);

			await host.RunAsync();
		}
	}
}