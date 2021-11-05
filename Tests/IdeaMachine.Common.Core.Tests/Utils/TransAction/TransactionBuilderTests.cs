using System;
using System.Threading;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.Transaction;
using NUnit.Framework;

namespace IdeaMachine.Common.Core.Tests.Utils.Transaction
{
	[TestFixture]
	public class TransactionBuilderTests
	{
		[Test]
		public async Task TransactionShouldWorkForHappyPath()
		{
			var transaction = new ParallelTransactionBuilder()
				.RegisterAction(() => Task.FromResult(true))
				.WithCompensation(() =>
				{
					Assert.Fail();
					return Task.CompletedTask;
				});

			await transaction.Execute();
		}

		[Test]
		public async Task TransactionShouldCompensateForFailure()
		{
			var transaction = new ParallelTransactionBuilder()
				.RegisterAction(() => throw new Exception())
				.WithCompensation(() =>
				{
					Assert.Pass();
					return Task.CompletedTask;
				});

			await transaction.Execute();
		}

		[Test]
		public async Task TransactionShouldCompensateForAnyFailure()
		{
			var compensatedActions = 0;

			var transaction = new ParallelTransactionBuilder()
				.RegisterAction(() => Task.CompletedTask)
				.WithCompensation(() =>
				{
					Interlocked.Increment(ref compensatedActions);
					return Task.CompletedTask;
				})
				.RegisterAction(() => throw new Exception())
				.WithCompensation(() =>
				{
					Interlocked.Increment(ref compensatedActions);
					return Task.CompletedTask;
				});

			await transaction.Execute();

			Assert.That(compensatedActions == 2);
		}
	}
}