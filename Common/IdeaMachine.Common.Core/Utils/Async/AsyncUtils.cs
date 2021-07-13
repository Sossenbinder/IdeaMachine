using System;
using System.Threading.Tasks;
using Serilog;

namespace IdeaMachine.Common.Core.Utils.Async
{
	public static class AsyncUtils
	{
		public static void RunSafe(Func<Task> taskRunner)
		{
			try
			{
				Task.Run(taskRunner).Wait();
			}
			catch (Exception e)
			{
				Log.Error(e, "Background task failed");
			}
		}

		public static T? RunSafe<T>(Func<Task<T>> taskRunner)
		{
			try
			{
				return Task.Run(taskRunner).Result;
			}
			catch (Exception e)
			{
				Log.Error(e, "Background task failed");

				return default;
			}
		}
	}
}