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
		private readonly List<IAsyncEnumerable<T>> _asyncStreams;

		public MergedAsyncEnumerable(IEnumerable<IAsyncEnumerable<T>> asyncStreams)
		{
			_asyncStreams = asyncStreams.ToList();
		}

		public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
		{
			return Consume(cancellationToken).GetAsyncEnumerator(cancellationToken);
		}

		private record AsyncIteratorResult(T Result, bool Done, int Index);

		private record IndexedEnumerator(int Index, IAsyncEnumerator<T> AsyncEnumerator)
		{
			// Advances an iterator, returning its value, details whether it is done, as well as its index
			public async Task<AsyncIteratorResult> Move()
			{
				var moreValuesAvailable = await AsyncEnumerator.MoveNextAsync();
				return new AsyncIteratorResult(AsyncEnumerator.Current, !moreValuesAvailable, Index);
			}
		}

		private static async Task<AsyncIteratorResult> CreateFillerTask(Task task)
		{
			await task;
			return new AsyncIteratorResult(default!, false, 0);
		}

		private async IAsyncEnumerable<T> Consume([EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			// Create a draining TaskCompletionSource
			var drainingTcs = new TaskCompletionSource();

			// Grab all underlying async enumerators and wrap them into index enumerators
			var indexedEnumerators = _asyncStreams
				.Select(x => x.GetAsyncEnumerator(cancellationToken))
				.Select((x, index) => new IndexedEnumerator(index, x))
				.ToList();

			// Kick off the respective first iteration of each iterator
			var runningTasks = indexedEnumerators
				.Select(x => x.Move())
				.ToList();

			// Keep track of finished iterators
			var finishedIterators = 0;

			try
			{
				while (finishedIterators != _asyncStreams.Count)
				{
					// Wait for the first task of all iterators to finish
					var completedTask = await Task.WhenAny(runningTasks);
					var (result, done, index) = completedTask.Result;

					if (done)
					{
						// If the task is done, it will receive the "empty" TaskCompletionSource
						// task. That way it will never finish in the next WhenAny call. This can be
						// nicer, but for now it works fair enough, and only inquires one "useless" task
						// allocation... (TODO: Implement this with proper handling for done tasks)
						finishedIterators++;
						runningTasks[index] = CreateFillerTask(drainingTcs.Task);
					}
					else
					{
						// If not done yet, return the result and move the running Task onwards for the current iterator
						yield return result;
						runningTasks[index] = indexedEnumerators[index].Move();
					}
				}
			}
			finally
			{
				// Now release the waiting remainder tasks
				drainingTcs.SetResult();
			}
		}
	}
}