using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.Async;

namespace IdeaMachine.Common.Core.Extensions.Async
{
	public static class AsyncEnumerableExtensions
	{
		public static Task ParallelAsync<T>([NotNull] this IEnumerable<T> enumerable, [NotNull] Func<T, Task> asyncAction, int maxParallelTasks = 50)
			=> ParallelAsyncScheduler<T>.Run(enumerable, asyncAction, maxParallelTasks);

		/// <summary>
		/// ValueTask ParallelAsync for compatibility reasons. Use if you know what you're doing, ValueTask isn't meant to be
		/// used like this...
		/// </summary>
		/// <param name="enumerable"></param>
		/// <param name="asyncAction"></param>
		/// <param name="maxParallelTasks"></param>
		/// <returns></returns>
		public static async ValueTask ParallelAsyncValueTask<T>([NotNull] this IEnumerable<T> enumerable, [NotNull] Func<T, ValueTask> asyncAction, int maxParallelTasks = 50)
			=> await enumerable.ParallelAsync(x => asyncAction(x).AsTask(), maxParallelTasks);

		public static Task<IEnumerable<TOut>> ParallelAsync<TIn, TOut>([NotNull] this IEnumerable<TIn> enumerable, [NotNull] Func<TIn, Task<TOut>> asyncAction, int maxParallelTasks = 50)
			=> ParallelAsyncScheduler<TIn, TOut>.Run(enumerable, asyncAction, maxParallelTasks);

		/// <summary>
		/// ValueTask ParallelAsync for compatibility reasons. Use if you know what you're doing, ValueTask isn't meant to be
		/// used like this...
		/// </summary>
		/// <param name="enumerable"></param>
		/// <param name="asyncAction"></param>
		/// <param name="maxParallelTasks"></param>
		/// <returns></returns>
		public static async ValueTask<IEnumerable<TOut>> ParallelAsyncValueTask<TIn, TOut>([NotNull] this IEnumerable<TIn> enumerable, [NotNull] Func<TIn, ValueTask<TOut>> asyncAction, int maxParallelTasks = 50)
			=> await enumerable.ParallelAsync(x => asyncAction(x).AsTask(), maxParallelTasks);
	}
}