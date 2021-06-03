using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Utils.Async
{
	public class ParallelAsyncScheduler<TIn>
	{
		private readonly ConcurrentQueue<TIn> _concurrentQueue;

		private readonly Func<TIn, Task> _asyncAction;

		private readonly int _parallelTaskCount;

		private ParallelAsyncScheduler(
			IEnumerable<TIn> dataSet,
			Func<TIn, Task> asyncAction,
			int parallelTaskCount = 30)
		{
			_concurrentQueue = new ConcurrentQueue<TIn>(dataSet);
			_asyncAction = asyncAction;
			_parallelTaskCount = parallelTaskCount > _concurrentQueue.Count ? _concurrentQueue.Count : parallelTaskCount;
		}

		public static Task Run(
			IEnumerable<TIn> dataSet,
			Func<TIn, Task> asyncAction,
			int parallelTaskCount = 30)
		{
			var parallelAsyncRunner = new ParallelAsyncScheduler<TIn>(dataSet, asyncAction, parallelTaskCount);

			return parallelAsyncRunner.RunInternal();
		}

		private async Task RunInternal()
		{
			var taskRunners = new List<ParallelAsyncRunner<TIn>>();
			for (var i = 0; i < _parallelTaskCount; ++i)
			{
				taskRunners.Add(new ParallelAsyncRunner<TIn>(_concurrentQueue, _asyncAction));
			}

			var runners = taskRunners
				.Select(x => x.Run())
				.ToList();

			var results = await Task.WhenAll(runners);

			var exceptions = results
				.Where(x => x != null)
				.SelectMany(x => x!)
				.ToList();

			if (exceptions.Any())
			{
				throw new AggregateException(exceptions);
			}
		}
	}

	public class ParallelAsyncScheduler<TIn, TOut>
	{
		private readonly ConcurrentQueue<TIn> _concurrentQueue;

		private readonly Func<TIn, Task<TOut>> _asyncAction;

		private readonly int _parallelTaskCount;

		private ParallelAsyncScheduler(
			IEnumerable<TIn> dataSet,
			Func<TIn, Task<TOut>> asyncAction,
			int parallelTaskCount = 30)
		{
			_concurrentQueue = new ConcurrentQueue<TIn>(dataSet);
			_asyncAction = asyncAction;
			_parallelTaskCount = parallelTaskCount > _concurrentQueue.Count ? _concurrentQueue.Count : parallelTaskCount;
		}

		public static Task<IEnumerable<TOut>> Run(
			IEnumerable<TIn> dataSet,
			Func<TIn, Task<TOut>> asyncAction,
			int parallelTaskCount = 30)
		{
			var parallelAsyncRunner = new ParallelAsyncScheduler<TIn, TOut>(dataSet, asyncAction, parallelTaskCount);

			return parallelAsyncRunner.RunInternal();
		}

		private async Task<IEnumerable<TOut>> RunInternal()
		{
			var taskRunners = new List<ParallelAsyncRunner<TIn, TOut>>();
			for (var i = 0; i < _parallelTaskCount; ++i)
			{
				taskRunners.Add(new ParallelAsyncRunner<TIn, TOut>(_concurrentQueue, _asyncAction));
			}

			var runners = taskRunners
				.Select(x => x.Run())
				.ToList();

			var parallelResults = await Task.WhenAll(runners);

			var exceptions = parallelResults
				.Where(x => x.Exceptions != null)
				.Select(x => x.Exceptions)
				.Where(x => x.Any())
				.SelectMany(x => x)
				.ToList();

			if (exceptions.Any())
			{
				throw new AggregateException(exceptions);
			}

			return parallelResults.SelectMany(x => x.Results);
		}
	}
}