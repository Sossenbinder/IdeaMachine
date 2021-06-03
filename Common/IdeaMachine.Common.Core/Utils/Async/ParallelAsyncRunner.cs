using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Utils.Async
{
	public record ParallelResult<T>(IEnumerable<Exception> Exceptions, IEnumerable<T> Results);

	public class ParallelAsyncRunner<T>
	{
		private readonly ConcurrentQueue<T> _workItemQueue;

		private readonly Func<T, Task> _asyncAction;

		public ParallelAsyncRunner(
			ConcurrentQueue<T> workItemQueue,
			Func<T, Task> asyncAction)
		{
			_workItemQueue = workItemQueue;
			_asyncAction = asyncAction;
		}

		public async Task<List<Exception>?> Run()
		{
			List<Exception>? exceptions = null;

			while (!_workItemQueue.IsEmpty)
			{
				var receivedWorkItem = _workItemQueue.TryDequeue(out var workItem);

				if (!receivedWorkItem)
				{
					continue;
				}

				if (workItem == null)
				{
					continue;
				}

				try
				{
					await _asyncAction(workItem);
				}
				catch (Exception e)
				{
					exceptions ??= new List<Exception>();

					exceptions.Add(e);
				}
			}

			return exceptions;
		}
	}

	public class ParallelAsyncRunner<TIn, TOut>
	{
		private readonly ConcurrentQueue<TIn> _workItemQueue;

		private readonly Func<TIn, Task<TOut>> _asyncAction;

		public ParallelAsyncRunner(
			ConcurrentQueue<TIn> workItemQueue,
			Func<TIn, Task<TOut>> asyncAction)
		{
			_workItemQueue = workItemQueue;
			_asyncAction = asyncAction;
		}

		public async Task<ParallelResult<TOut>> Run()
		{
			var results = new List<TOut>();
			List<Exception>? exceptions = null;

			while (!_workItemQueue.IsEmpty)
			{
				var receivedWorkItem = _workItemQueue.TryDequeue(out var workItem);

				if (!receivedWorkItem)
				{
					continue;
				}

				if (workItem == null)
				{
					continue;
				}

				try
				{
					results.Add(await _asyncAction(workItem));
				}
				catch (Exception e)
				{
					exceptions ??= new List<Exception>();

					exceptions.Add(e);
				}
			}

			return new ParallelResult<TOut>(exceptions!, results);
		}
	}
}