using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Extensions;
using Microsoft.Extensions.Logging;

namespace IdeaMachine.Common.Core.Utils.Tasks
{
	public class FireAndForgetTask
	{
		public Task UnderlyingTask { get; init; }

		private FireAndForgetTask(Task underlyingTask)
		{
			UnderlyingTask = underlyingTask;
		}

		/// <summary>
		/// Raises a fire and forget task
		/// </summary>
		/// <param name="taskGenerator"></param>
		/// <param name="logger"></param>
		/// <returns></returns>
		public static FireAndForgetTask Run(
			Func<Task> taskGenerator,
			ILogger? logger)
		{
			var task = RunInternal(taskGenerator, logger);
			return new FireAndForgetTask(task);
		}

		/// <summary>
		/// Raises a fire and forget task on the ThreadPool
		/// </summary>
		/// <param name="taskGenerator"></param>
		/// <param name="logger"></param>
		/// <returns></returns>
		public static FireAndForgetTask RunThreadPool(
			Func<Task> taskGenerator,
			ILogger? logger)
		{
			var task = Task.Run(() => RunInternal(taskGenerator, logger));
			return new FireAndForgetTask(task);
		}

		private static async Task RunInternal(
			Func<Task> taskGenerator,
			ILogger? logger)
		{
			try
			{
				await taskGenerator();
			}
			catch (Exception e)
			{
				logger?.LogException(e);
			}
		}
	}
}