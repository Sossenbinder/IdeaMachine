using System.Threading.Tasks;
using IdeaMachine.Common.RuntimeSerialization.Serialize;
using IdeaMachine.Service.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdeaMachine.AccountService
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var host = CommonWebHostBuilder.CreateGrpcService<Startup>(args);

			host.Services.GetRequiredService<SerializationModelBinderService>().InitializeProtoSerializer();

			await host.RunAsync();
		}
	}
}