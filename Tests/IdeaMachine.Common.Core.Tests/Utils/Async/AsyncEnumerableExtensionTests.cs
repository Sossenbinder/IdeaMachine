using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Extensions.Async;
using NUnit.Framework;

namespace IdeaMachine.Common.Core.Tests.Utils.Async
{
	[TestFixture]
	public class AsyncEnumerableExtensionTests
	{
		[Test]
		public async Task ShouldWorkForHappyPath()
		{
			var collection = new List<int>() { 2, 5, 6, 7, 84 };
			var resultCollection = new List<int>();

			await collection.ConsumeInParallel(
				async x =>
				{
					await Task.Delay(100);
					return x + 1;
				},
				async x =>
				{
					await Task.Delay(100);
					resultCollection.Add(x);
				});

			resultCollection.Sort();

			for (var i = 0; i < collection.Count - 1; ++i)
			{
				Assert.True(resultCollection[i] == collection[i] + 1);
			}
		}
	}
}