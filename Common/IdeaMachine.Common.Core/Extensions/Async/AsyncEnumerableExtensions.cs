using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.Async;

namespace IdeaMachine.Common.Core.Extensions.Async
{
	public static class AsyncEnumerableExtensions
	{
		public static Task ParallelAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> asyncAction, int maxParallelTasks = 50)
			=> ParallelAsyncScheduler<T>.Run(enumerable, asyncAction, maxParallelTasks);

		/// <summary>
		/// ValueTask ParallelAsync for compatibility reasons. Use if you know what you're doing, ValueTask isn't meant to be
		/// used like this...
		/// </summary>
		/// <param name="enumerable"></param>
		/// <param name="asyncAction"></param>
		/// <param name="maxParallelTasks"></param>
		/// <returns></returns>
		public static async ValueTask ParallelAsyncValueTask<T>(this IEnumerable<T> enumerable, Func<T, ValueTask> asyncAction, int maxParallelTasks = 50)
			=> await enumerable.ParallelAsync(x => asyncAction(x).AsTask(), maxParallelTasks);

		public static Task<IEnumerable<TOut>> ParallelAsync<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, Task<TOut>> asyncAction, int maxParallelTasks = 50)
			=> ParallelAsyncScheduler<TIn, TOut>.Run(enumerable, asyncAction, maxParallelTasks);

		/// <summary>
		/// ValueTask ParallelAsync for compatibility reasons. Use if you know what you're doing, ValueTask isn't meant to be
		/// used like this...
		/// </summary>
		/// <param name="enumerable"></param>
		/// <param name="asyncAction"></param>
		/// <param name="maxParallelTasks"></param>
		/// <returns></returns>
		public static async ValueTask<IEnumerable<TOut>> ParallelAsyncValueTask<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, ValueTask<TOut>> asyncAction, int maxParallelTasks = 50)
			=> await enumerable.ParallelAsync(x => asyncAction(x).AsTask(), maxParallelTasks);

		public static async Task ConsumeInParallel<TIn, TTaskRet>(
			this IEnumerable<TIn> collection,
			Func<TIn, Task<TTaskRet>> collectionTaskTransformer,
			Func<TTaskRet, Task> collectionConsumer,
			int consumerCount = 2,
			CancellationToken cancellationToken = default)
		{
			var channel = Channel.CreateUnbounded<TTaskRet>();
			var consumers = Enumerable.Range(0, consumerCount).Select(_ => Consume(channel.Reader, collectionConsumer, cancellationToken)).ToList();

			var writer = channel.Writer;
			await foreach (var item in RunAsyncEnumerable(collection.ToList(), collectionTaskTransformer).WithCancellation(cancellationToken))
			{
				await writer.WriteAsync(item, cancellationToken);
			}
			writer.Complete();

			await Task.WhenAll(consumers);
		}

		private static async IAsyncEnumerable<TTaskRet> RunAsyncEnumerable<TIn, TTaskRet>(
			IEnumerable<TIn> collection,
			Func<TIn, Task<TTaskRet>> collectionTaskTransformer)
		{
			var tasks = collection.Select(collectionTaskTransformer).ToList();

			while (tasks.Count > 0)
			{
				var winner = await Task.WhenAny(tasks);
				yield return winner.Result;
				tasks.Remove(winner);
			}
		}

		private static async Task Consume<T>
		(ChannelReader<T> reader,
			Func<T, Task> consumerFunc,
			CancellationToken ct)
		{
			while (await reader.WaitToReadAsync(ct) && !ct.IsCancellationRequested)
			{
				while (reader.TryRead(out var value))
				{
					await consumerFunc(value);
				}
			}
		}
	}
}