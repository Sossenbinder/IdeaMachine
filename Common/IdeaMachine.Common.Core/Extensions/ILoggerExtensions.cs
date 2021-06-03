using System;
using Microsoft.Extensions.Logging;

namespace IdeaMachine.Common.Core.Extensions
{
	public static class ILoggerExtensions
	{
		public static void LogException(this ILogger logger, Exception exception, string message = "", params object[] args)
		{
			logger.LogError(exception, message, args);
		}

		public static void LogException<T>(this ILogger<T> logger, Exception exception, string message = "", params object[] args)
		{
			logger.LogError(exception, message, args);
		}
	}
}