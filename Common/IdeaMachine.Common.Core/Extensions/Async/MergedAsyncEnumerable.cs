using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Extensions.Async
{
    public class MergedAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
	    private readonly IEnumerable<IAsyncEnumerable<T>> _asyncStreams;

	    public MergedAsyncEnumerable(IEnumerable<IAsyncEnumerable<T>> asyncStreams)
	    {
		    _asyncStreams = asyncStreams;
	    }
		
		public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
		{
			return Consume(cancellationToken).GetAsyncEnumerator(cancellationToken);
		}

		private record AsyncIteratorResult(T Result, bool Done, int Index);

		private class TaskInformation
		{
			private readonly int _index;

			private readonly IAsyncEnumerator<T> _enumerator;

			public TaskInformation(int index, IAsyncEnumerator<T> enumerator, CancellationToken ct)
			{
				_index = index;
				_enumerator = enumerator;
			}

			public async Task<AsyncIteratorResult> Move()
			{
				var moreValuesAvailable = await _enumerator.MoveNextAsync().AsTask();
				return new AsyncIteratorResult(_enumerator.Current, !moreValuesAvailable, _index);
			}
		}

		private static async Task<AsyncIteratorResult> CreateFillerTask(CancellationToken token)
		{
			await Task.Delay(-1, token).IgnoreTaskCancelledException();
			return new AsyncIteratorResult(default!, false, 0);
		}

		private async IAsyncEnumerable<T> Consume([EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var drainingCts = new CancellationTokenSource();

			var asyncEnumerators = _asyncStreams
				.Select(x => x.GetAsyncEnumerator(cancellationToken))
				.ToList();

			var taskList = asyncEnumerators
				.Select((x, index) => new TaskInformation(index, x, drainingCts.Token))
				.ToList();

			var runningTasks = taskList
				.Select(x => x.Move())
				.ToList();

			var finishedIterators = 0;

			try
			{
				while (finishedIterators != asyncEnumerators.Count)
				{
					var completedTask = await Task.WhenAny(runningTasks);
					var (result, done, index) = completedTask.Result;

					if (done)
					{
						finishedIterators++;
						runningTasks[index] = CreateFillerTask(drainingCts.Token);
					}
					else
					{
						//Console.WriteLine($"Yielding {result} for index {index} - More values available? {!done}");
						yield return result;
						runningTasks[index] = taskList[index].Move();
					}
				}
			}
			finally
			{
				drainingCts.Cancel();
			}
		}
    }
}
