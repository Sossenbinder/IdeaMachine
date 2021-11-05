using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Serilog;

namespace IdeaMachine.Common.Core.Utils.Transaction
{
	/// <summary>
	/// Executes all actions in parallel, and if an error occurs, executes all compensations in parallel
	/// </summary>
	public class ParallelTransactionBuilder : AbstractUntypedTransactionBuilder
	{
		private readonly CallerInfo _callerInfo;

		public ParallelTransactionBuilder(
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{
			_callerInfo = new CallerInfo(memberName, sourceFilePath, sourceLineNumber);
		}

		public static ParallelTransactionBuilder Initialize(Func<Task> action)
		{
			var builder = new ParallelTransactionBuilder();
			builder.RegisterAction(action);
			return builder;
		}

		public override async Task Execute()
		{
			try
			{
				await Task.WhenAll(Handlers.Select(x => x.Action()));
			}
			catch (Exception e)
			{
				Log.Logger.Error(e, "Exception occured while executing a transaction for caller {CallerInfo}, line {SourceLineNumber}", _callerInfo.MemberName, _callerInfo.SourceLineNumber);

				await Task.WhenAll(Handlers.Select(x => x.CompensationAction()));
			}
		}
	}
}