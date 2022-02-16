using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Common.Core.Utils.Streams;
using NUnit.Framework;

namespace IdeaMachine.Common.Core.Tests.Utils.Streams
{
	[TestFixture]
	public class MultiplexWriteOnlyStreamTests
	{
		[Test]
		public async Task MultiplexShouldWorkForHappyPath()
		{
			const string str = "12345678";

			await using var firstStream = new MemoryStream();
			await using var secondStream = new MemoryStream();

			var buffer = Convert.FromBase64String(str);
			await using (var multiplexStream = new MultiplexWriteOnlyStream(firstStream, secondStream))
			{
				await multiplexStream.WriteAsync(buffer, 0, buffer.Length);
			}

			// Both sinks should now have data
			Assert.True(firstStream.Length != 0);
			Assert.True(secondStream.Length != 0);

			var firstBuffer = new byte[buffer.Length];
			var secondBuffer = new byte[buffer.Length];
			Assert.True(await firstStream.ReadAsync(firstBuffer, 0, (int)firstStream.Length) != 0);
			Assert.True(await secondStream.ReadAsync(secondBuffer, 0, (int)secondStream.Length) != 0);
			var firstSinkData = Convert.ToBase64String(firstBuffer);
			var secondSinkData = Convert.ToBase64String(secondBuffer);

			Assert.AreEqual(str, firstSinkData);
			Assert.AreEqual(str, secondSinkData);
		}
	}
}