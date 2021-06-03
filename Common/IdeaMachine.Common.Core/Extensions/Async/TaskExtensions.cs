using System;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Extensions.Async
{
	public static class TaskExtensions
	{
		public static async Task WaitAsync(this Task task, TimeSpan timeSpan)
		{
			await task;
			await Task.Delay(timeSpan);
		}

		public static async Task WaitAsync<T>(this Task<T> task, TimeSpan timeSpan)
		{
			await task;
			await Task.Delay(timeSpan);
		}

		public static Task IgnoreTaskCancelledException(this Task task)
		{
			return task.ContinueWith(continuedTask => continuedTask.Exception?.Handle(ex => ex is TaskCanceledException));
		}
	}
}