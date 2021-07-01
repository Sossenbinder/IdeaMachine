using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace IdeaMachine.Common.Logging.Log
{
	public static class LogProvider
	{
		public static ILogger CreateLogger(IConfiguration configuration)
		{
			var defaultLoggerConfiguration = new LoggerConfiguration()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.Enrich.FromLogContext()
				.WriteTo.Console();

			var seqServer = configuration["Seq_Server"];
			if (seqServer != null)
			{
				defaultLoggerConfiguration = defaultLoggerConfiguration.WriteTo.Seq(seqServer);
			}

			var logger = Serilog.Log.Logger = defaultLoggerConfiguration.CreateLogger();

			logger.Information("Logger initialized");

			return logger;
		}
	}
}