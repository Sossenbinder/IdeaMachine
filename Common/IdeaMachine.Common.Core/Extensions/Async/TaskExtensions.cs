using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Extensions.Async
{
	public static class TaskExtensions
	{
		public static Task IgnoreTaskCancelledException(this Task task)
		{
			return task.ContinueWith(continuedTask => continuedTask.Exception?.Handle(ex => ex is TaskCanceledException));
		}

		public static Task<TNew?> As<T, TNew>(this Task<T> originalTask)
			where TNew : class, T
		{
			return originalTask.ContinueWith(x => x.Result as TNew);
		}
	}
}