using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Extensions;

namespace IdeaMachine.Common.Core.Utils.Retry
{
	public static class RetryStrategy
	{
		#region TaskBased

		public static Task DoRetry(
			Action action,
			Func<int, Task>? onRetry = null,
			int msBetweenRetries = 0,
			int retryCount = 10)
		{
			return RetryInternal(action, msBetweenRetries, retryCount, onRetry);
		}

		public static Task DoRetryExponential(
			Action action,
			Func<int, Task>? onRetry = null,
			int msBetweenRetries = 1000,
			int retryScalingFactor = 2,
			int retryCount = 10)
		{
			return RetryInternal(action, msBetweenRetries, retryCount, onRetry, currentWaitTime => currentWaitTime * retryScalingFactor);
		}

		#endregion TaskBased

		#region ActionBased

		public static Task DoRetry(
			Action action,
			Action<int>? onRetry = null,
			int msBetweenRetries = 0,
			int retryCount = 10)
		{
			return RetryInternal(action, msBetweenRetries, retryCount, onRetry.MakeTaskCompatible());
		}

		public static Task DoRetryExponential(
			Action action,
			Action<int>? onRetry = null,
			int msBetweenRetries = 1000,
			int retryScalingFactor = 2,
			int retryCount = 10)
		{
			return RetryInternal(action, msBetweenRetries, retryCount, onRetry.MakeTaskCompatible(), currentWaitTime => currentWaitTime * retryScalingFactor);
		}

		#endregion ActionBased

		private static async Task RetryInternal(
			Action action,
			int msBetweenRetries,
			int retryCount,
			Func<int, Task>? onRetry = null,
			Func<int, int>? waitTimeTransformer = null)
		{
			var msUntilNextRetry = msBetweenRetries;
			var subExceptions = new List<Exception>();

			for (var i = 0; i < retryCount; ++i)
			{
				try
				{
					action();
					return;
				}
				catch (Exception exc)
				{
					subExceptions.Add(exc);
				}

				if (msBetweenRetries > 0)
				{
					msBetweenRetries = waitTimeTransformer?.Invoke(msBetweenRetries) ?? msBetweenRetries;
					await Task.Delay(msUntilNextRetry);
				}

				if (onRetry != null)
				{
					await onRetry(i);
				}
			}

			throw new AggregateException(subExceptions);
		}
	}
}