using System.Linq;
using IdeaMachine.Common.Core.Extensions;
using NUnit.Framework;

namespace IdeaMachine.Common.Core.Tests.Extensions
{
	internal class TestClass
	{
		public int Item { get; set; }
	}

	[TestFixture]
	public class EnumerableExtensionsTests
	{
		[Test]
		public void ApplyShouldWorkAsExpected()
		{
			var numbers = Enumerable.Range(0, 10).Select(x => new TestClass() { Item = x }).ToList();

			_ = numbers.Apply(x =>
			{
				x.Item++;
			}).ToList();

			for (var i = 0; i < numbers.Count - 1; ++i)
			{
				Assert.That(numbers[i].Item == i + 1);
			}
		}
	}
}