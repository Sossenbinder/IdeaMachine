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

			private readonly CancellationToken _ct;

			private ValueTask<bool> _task;

			public TaskInformation(int index, IAsyncEnumerator<T> enumerator, CancellationToken ct)
			{
				_index = index;
				_enumerator = enumerator;
				_ct = ct;
				_task = enumerator.MoveNextAsync();
			}

			public async Task<AsyncIteratorResult> Move()
			{
				var moreValuesAvailable = await _task;

				_task = moreValuesAvailable ? _enumerator.MoveNextAsync() : CreateFillerTask(_ct);

				Console.WriteLine($"Index {_index} returning value {_enumerator.Current} - More values available? {moreValuesAvailable}");
				return new AsyncIteratorResult(_enumerator.Current, !moreValuesAvailable, _index);
			}

			private static async ValueTask<bool> CreateFillerTask(CancellationToken token)
			{
				await Task.Delay(-1, token).IgnoreTaskCancelledException();
				return false;
			}
		}

		private async IAsyncEnumerable<T> Consume([EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var drainingCts = new CancellationTokenSource();

			var asyncEnumerators = _asyncStreams
				.Select(x => x.GetAsyncEnumerator(cancellationToken))
				.ToList();

			var asyncEnumeratorsCount = asyncEnumerators.Count;

			var taskList = asyncEnumerators
				.Select((x, index) => new TaskInformation(index, x, drainingCts.Token))
				.ToList();

			var runningTasks = taskList
				.Select(x => x.Move())
				.ToList();

			var finishedIterators = 0;

			while (taskList.Any())
			{
				var completedTask = await Task.WhenAny(runningTasks);
				var (result, done, index) = completedTask.Result;

				if (done)
				{
					finishedIterators++;
				}
				else
				{
					Console.WriteLine($"Yielding {result} for index {index} - More values available? {!done}");
					yield return result;
					runningTasks[index] = taskList[index].Move();
				}

				if (finishedIterators == asyncEnumeratorsCount)
				{
					drainingCts.Cancel();
					yield break;
				}
			}
		}
    }
}
