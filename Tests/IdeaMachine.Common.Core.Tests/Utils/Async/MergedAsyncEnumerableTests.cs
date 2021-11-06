using System.Collections.Generic;
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
		    var asyncSet = Enumerable.Range(0, 5).Select(x => TestAsyncEnumerable(5)).ToList();
			var merger = new MergedAsyncEnumerable<int>(asyncSet);

			var results = new List<int>();
			await foreach (var item in merger)
			{
				results.Add(item);
			}

			Assert.AreEqual(25, results.Count);
	    }

	    private static async IAsyncEnumerable<int> TestAsyncEnumerable( int results)
	    {
		    for (var i = 0; i < results; ++i)
		    {
			    await Task.Delay(100);
			    yield return await Task.FromResult(i);
		    }
	    }
    }
}
