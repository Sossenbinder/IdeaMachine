using System.Collections.Generic;

namespace IdeaMachine.Common.Logging
{
	public static class LoggerConstants
	{
		public static string LoggingAzureTableStorageTableName = "Logs";

		public static string LoggingFilePath = "/hostlogs";

		public static int LoggingMaxHistoryCount = 15;

		public static Dictionary<string, string> GetConstantsAsInMemoryDict()
		{
			return new()
			{
				{ nameof(LoggingAzureTableStorageTableName), LoggingAzureTableStorageTableName },
				{ nameof(LoggingFilePath), LoggingFilePath },
				{ nameof(LoggingMaxHistoryCount), LoggingMaxHistoryCount.ToString() }
			};
		}
	}
}