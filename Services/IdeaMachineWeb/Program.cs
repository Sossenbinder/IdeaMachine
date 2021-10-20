using System.Threading.Tasks;
using IdeaMachine.Service.Base;
using Microsoft.Extensions.Hosting;

namespace IdeaMachineWeb
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var built = CommonWebHostBuilder.Create<Startup>(args);

			await built.RunAsync();
		}
	}
}