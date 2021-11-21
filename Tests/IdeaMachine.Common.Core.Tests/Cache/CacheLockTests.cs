using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Cache.Implementations;
using IdeaMachine.Common.Core.Extensions.Async;
using NUnit.Framework;

namespace IdeaMachine.Common.Core.Tests.Cache
{
	[TestFixture]
    public class CacheLockTests
	{
		private MemoryCache<string, TestItem> _cache = null!;

		[SetUp]
		protected void SetUp()
		{
			_cache = new MemoryCache<string, TestItem>();
		}

		[Test]
		public async Task LockShouldWorkForHappyPath()
		{
			const string key = "Key_1";

			var testItem = new TestItem(5);

			_cache.Set(key, testItem);

			var lockedCacheItem = await _cache.GetLocked(key);
			await lockedCacheItem!.Release();

			var secondItem = await _cache.GetLocked(key);

			Assert.True(secondItem!.Value.Counter == testItem.Counter);
		}

		[Test]
		public async Task LockShouldBeUniqueForHappyPath()
		{
			const string key = "Key_1";

			var testItem = new TestItem(5);

			_cache.Set(key, testItem);

			var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

			var lockedCacheItem = await _cache.GetLocked(key);
			var secondWaiter = _cache.GetLocked(key);

			await Task.Delay(500, cts.Token);

			Assert.False(secondWaiter.IsCompleted);

			await lockedCacheItem!.Release();
			
			var firstTask = await Task.WhenAny(secondWaiter, new Func<Task>(async () => await cts.Token)());

			if (firstTask == secondWaiter)
			{
				Assert.Pass();
			}

			Assert.Fail();
		}

		[Test]
		public void LockShouldThrowIfNoItemPresentForKey()
		{
			const string key = "Key_1";
			
			Assert.ThrowsAsync<KeyNotFoundException>(() => _cache.GetLocked(key));
		}

		[Test]
		public async Task LockShouldProperlyDispose()
		{
			const string key = "Key_1";

			var testItem = new TestItem(5);
			const int newVal = 3;

			_cache.Set(key, testItem);

			async Task DoStuff()
			{
				await using (var lockedItem = await _cache.GetLocked(key))
				{
					lockedItem.Value.Counter = newVal;
				}
			}

			var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

			var doer = DoStuff();
			var task = await Task.WhenAny(doer, new Func<Task>(async () => await cts.Token)());

			var item = _cache.Get(key);

			Assert.True(task == doer);
			Assert.True(item.Counter == newVal);
		}
	}
}
