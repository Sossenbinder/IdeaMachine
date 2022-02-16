using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Utils.Async
{
	public class ParallelAsyncEnumerableScheduler<TSource, TTransformed>
	{
		private readonly IEnumerable<TSource> _source;

		private readonly Func<TSource, Task<TTransformed>> _transformer;

		private readonly int _limit;

		private readonly CancellationToken _cancellationToken;

		public ParallelAsyncEnumerableScheduler(
			IEnumerable<TSource> source,
			Func<TSource, Task<TTransformed>> transformer,
			CancellationToken cancellationToken,
			int limit = 10)
		{
			_source = source;
			_transformer = transformer;
			_cancellationToken = cancellationToken;
			_limit = limit;
		}

		public async Task Run()
		{
			var concurrentQueue = new ConcurrentQueue<TTransformed>();
			await foreach (var result in RunAsyncEnumerable(_source.Select<TSource, Func<Task<TTransformed>>>(x => () => _transformer(x))).WithCancellation(_cancellationToken))
			{
				concurrentQueue.Enqueue(result);
			}
		}

		private async IAsyncEnumerable<TTransformed> RunAsyncEnumerable(
			IEnumerable<Func<Task<TTransformed>>> source)
		{
			var tasks = source.Select(x => x()).ToList();

			while (tasks.Any() && !_cancellationToken.IsCancellationRequested)
			{
				var finishedTask = await Task.WhenAny(tasks);
				yield return finishedTask.Result;
				tasks.Remove(finishedTask);
			}
		}
	}
}