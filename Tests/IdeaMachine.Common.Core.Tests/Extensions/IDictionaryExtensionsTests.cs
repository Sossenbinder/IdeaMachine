using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Extensions;
using NUnit.Framework;

namespace IdeaMachine.Common.Core.Tests.Extensions
{
	[TestFixture]
	public class IDictionaryExtensionsTests
	{
		[Test]
		public void TryGetValueTypedShouldWorkProperly()
		{
			const int targetKey = 5;
			const int targetVal = 6;

			IDictionary<object, object> dict = new Dictionary<object, object> { { targetKey, targetVal } };

			if (dict.TryGetValueTyped(targetKey, out int val))
			{
				Assert.AreEqual(targetVal, val);
				Assert.Pass();
			}

			Assert.Fail();
		}
	}
}