using System;
using System.Threading.Tasks;
using Serilog;

namespace IdeaMachine.Common.Core.Extensions.Async
{
	public static class TaskCompat
	{
		public static void CallSync(Func<Task> taskGenerator)
		{
			try
			{
				taskGenerator()
					.ConfigureAwait(false)
					.GetAwaiter()
					.GetResult();
			}
			catch (Exception e)
			{
				Log.Logger.Error(e, "Synchronous task invocation failed");
			}
		}
	}
}