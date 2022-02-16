using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using IdeaMachine.Common.Eventing.Events;
using IdeaMachine.Common.Eventing.Extensions;
using NUnit.Framework;

namespace IdeaMachine.Common.Eventing.Tests.Extensions
{
	// ReSharper disable once InconsistentNaming
	[TestFixture]
	public class IEventExtensionsTests
	{
		[Test]
		public async Task WaitEventShouldWorkAsExpectedWithDelay()
		{
			var @event = new AsyncEvent();

			const int msDelay = 100;
			var cts = new CancellationTokenSource(msDelay);
			var sw = Stopwatch.StartNew();
			var waitResult = await @event.Wait(cancellationToken: cts.Token);
			sw.Stop();
			Assert.True(sw.ElapsedMilliseconds >= msDelay);
			Assert.False(waitResult);
		}

		[Test]
		public async Task WaitEventShouldWorkAsExpectedWithTimeout()
		{
			var @event = new AsyncEvent();

			const int msDelay = 100;
			var sw = Stopwatch.StartNew();
			var waitResult = await @event.Wait(TimeSpan.FromMilliseconds(msDelay));
			sw.Stop();
			Assert.True(sw.ElapsedMilliseconds >= msDelay);
			Assert.False(waitResult);
		}

		[Test]
		public async Task WaitEventShouldCallbackOnTimeout()
		{
			var @event = new AsyncEvent();

			await @event.Wait(TimeSpan.FromMilliseconds(100), () =>
			{
				Assert.Pass();
				return ValueTask.CompletedTask;
			});

			Assert.Fail();
		}

		[Test]
		public async Task WaitEventShouldWorkForPublishedEvent()
		{
			var @event = new AsyncEvent();

			_ = ((Func<Task>)(async () =>
			{
				await Task.Delay(100);
				await @event.Raise();
			}))();

			var cts = new CancellationTokenSource(500);

			var result = await @event.Wait(cancellationToken: cts.Token);
			Assert.True(result);
		}
	}
}