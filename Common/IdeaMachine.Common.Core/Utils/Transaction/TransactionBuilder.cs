using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Serilog;
using Serilog.Core;

namespace IdeaMachine.Common.Core.Utils.Transaction
{
	public class TransactionBuilder
	{
		private readonly List<ActionHandler> _handlers;

		private readonly CallerInfo _callerInfo;

		public TransactionBuilder(
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{
			_handlers = new();

			_callerInfo = new(memberName, sourceFilePath, sourceLineNumber);
		}

		public TransactionBuilder(
			List<ActionHandler> actionHandlers,
			CallerInfo callerInfo)
		{
			_handlers = actionHandlers;
			_callerInfo = callerInfo;
		}

		public ActionBuilder RegisterAction(Func<Task> action)
		{
			return new(action, _handlers, _callerInfo);
		}

		public async Task Execute()
		{
			try
			{
				await Task.WhenAll(_handlers.Select(x => x.Action()));
			}
			catch (Exception e)
			{
				Log.Logger.Error(e, "Exception occured while executing a transaction for caller {CallerInfo}, line {SourceLineNumber}", _callerInfo.MemberName, _callerInfo.SourceLineNumber);

				await Task.WhenAll(_handlers.Select(x => x.CompensationAction()));
			}
		}
	}

	public class ActionBuilder
	{
		private readonly Func<Task> _action;

		private readonly List<ActionHandler> _handlers;

		private readonly CallerInfo _callerInfo;

		private Func<Task> _compensationAction = null!;

		internal ActionBuilder(
			Func<Task> action,
			List<ActionHandler> handlers,
			CallerInfo callerInfo)
		{
			_action = action;
			_handlers = handlers;
			_callerInfo = callerInfo;
		}

		public TransactionBuilder WithCompensation(Func<Task> compensationAction)
		{
			_compensationAction = compensationAction;

			_handlers.Add(new ActionHandler(_action, _compensationAction));

			return new TransactionBuilder(_handlers, _callerInfo);
		}
	}

	public record ActionHandler(Func<Task> Action, Func<Task> CompensationAction)
	{
		public Task Execute() => Action();

		public Task Compensate() => CompensationAction();
	}
}