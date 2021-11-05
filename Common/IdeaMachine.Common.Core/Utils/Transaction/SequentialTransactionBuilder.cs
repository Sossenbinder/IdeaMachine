using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Serilog;

namespace IdeaMachine.Common.Core.Utils.Transaction
{
	/// <summary>
	/// Executes all actions sequentially, and when an exception occurs, it will rewind the exceptions from that exceptions on
	/// </summary>
	public class SequentialTransactionBuilder : AbstractUntypedTransactionBuilder
	{
		private readonly CallerInfo _callerInfo;

		public SequentialTransactionBuilder(
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{
			_callerInfo = new CallerInfo(memberName, sourceFilePath, sourceLineNumber);
		}

		public static SequentialTransactionBuilder Initialize(Func<Task> action)
		{
			var builder = new SequentialTransactionBuilder();
			builder.RegisterAction(action);
			return builder;
		}

		public override async Task Execute()
		{
			var i = 0;
			try
			{
				for (; i < Handlers.Count; ++i)
				{
					await Handlers[i].Action();
				}
			}
			catch (Exception e)
			{
				Log.Error(e, "Exception occured while executing a transaction for caller {CallerInfo}, line {SourceLineNumber}", _callerInfo.MemberName, _callerInfo.SourceLineNumber);

				for (; i >= 0; --i)
				{
					await Handlers[i].CompensationAction();
				}
			}
		}
	}
}
