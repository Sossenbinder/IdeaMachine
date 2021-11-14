using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Extensions.Async;
using NUnit.Framework;

namespace IdeaMachine.Common.Core.Tests.Utils.Async
{
    [TestFixture]
    public class MergedAsyncEnumerableTests
    {
	    [Test]
	    public async Task MergedAsyncEnumerableShouldWorkForHappyPath()
	    {
		    const int waitTime = 100;

		    var asyncSet = Enumerable.Range(0, 5).Select(x => TestAsyncEnumerable(5, waitTime)).ToList();
			var merger = new MergedAsyncEnumerable<int>(asyncSet);

			var results = new List<int>();
			await foreach (var item in merger)
			{
				results.Add(item);
			}

			Assert.AreEqual(25, results.Count);
		}

	    [Test]
	    public async Task MergedAsyncEnumerableShouldBeFasterThanSequentialForHappyPath()
	    {
		    const int waitTime = 100;

		    var asyncSet = Enumerable.Range(0, 5).Select(x => TestAsyncEnumerable(5, waitTime)).ToList();
		    var merger = new MergedAsyncEnumerable<int>(asyncSet);

		    var mergedSw = Stopwatch.StartNew();
		    await foreach (var _ in merger)
		    {
		    }
		    mergedSw.Stop();

		    var sequentialSw = Stopwatch.StartNew();
		    foreach (var asyncEnumerable in asyncSet)
		    {
			    await foreach (var _ in asyncEnumerable)
			    {
			    }
			}
			sequentialSw.Stop();

			Assert.True(sequentialSw.ElapsedMilliseconds > 5 * 5 * waitTime);
			Assert.True(mergedSw.ElapsedMilliseconds < 2 * 5 * waitTime);
	    }

		private static async IAsyncEnumerable<int> TestAsyncEnumerable(int results, int waitTime)
	    {
		    for (var i = 0; i < results; ++i)
		    {
			    await Task.Delay(waitTime);
			    yield return await Task.FromResult(i);
		    }
	    }
    }
}
