using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace IdeaMachine.Service.Base.Extensions
{
	public static class WebHostBuilderExtensions
	{
		public static IWebHostBuilder ConfigureGrpc(this IWebHostBuilder webHostBuilder, int port = 11337)
		{
			return webHostBuilder
				.UseKestrel(options =>
				{
					// Compatibility to allow regular calls. Also, we only need 80 since services are meant to sit behind a TLS terminating proxy
					options.ListenAnyIP(80);

					// Grpc port listener
					options.ListenAnyIP(port, listenOptions =>
					{
						listenOptions.Protocols = HttpProtocols.Http2;
					});
				});
		}
	}
}