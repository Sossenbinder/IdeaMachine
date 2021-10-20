using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using IdeaMachine.Service.Base;

namespace IdeaMachine
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