using System.Collections.Generic;

namespace IdeaMachine.Common.Logging
{
	public static class LoggerConstants
	{
		public static string Logging_AzureTableStorage_TableName = "Logs";

		public static string Logging_File_Path = "/hostlogs";

		public static int Logging_MaxHistoryCount = 15;

		public static Dictionary<string, string> GetConstantsAsInMemoryDict()
		{
			return new()
			{
				{ nameof(Logging_AzureTableStorage_TableName), Logging_AzureTableStorage_TableName },
				{ nameof(Logging_File_Path), Logging_File_Path },
				{ nameof(Logging_MaxHistoryCount), Logging_MaxHistoryCount.ToString() }
			};
		}
	}
}