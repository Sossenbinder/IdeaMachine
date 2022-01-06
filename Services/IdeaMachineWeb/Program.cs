using System.Threading.Tasks;
using IdeaMachine.Common.RuntimeSerialization.Serialize;
using IdeaMachine.Service.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdeaMachineWeb
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var built = CommonWebHostBuilder.Create<Startup>(args);

			built.Services.GetRequiredService<SerializationModelBinderService>().InitializeProtoSerializer();

			await built.RunAsync();
		}
	}
}