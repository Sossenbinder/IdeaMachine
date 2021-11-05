using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Utils.Transaction
{
    public abstract class AbstractUntypedTransactionBuilder
    {
	    protected List<ActionHandler> Handlers { get; }

		protected AbstractUntypedTransactionBuilder()
		{
			Handlers = new List<ActionHandler>();
		}

		public CompensationBuilder RegisterAction(Func<Task> action)
		{
			return new CompensationBuilder(action, this);
		}

		public abstract Task Execute();

		public record ActionHandler(Func<Task> Action, Func<Task> CompensationAction);

		public class CompensationBuilder
		{
			private readonly Func<Task> _action;

			private readonly AbstractUntypedTransactionBuilder _builder;

			internal CompensationBuilder(
				Func<Task> action,
				AbstractUntypedTransactionBuilder builder)
			{
				_action = action;
				_builder = builder;
			}

			public AbstractUntypedTransactionBuilder WithCompensation(Func<Task> compensationAction)
			{
				var newActionHandler = new ActionHandler(_action, compensationAction);

				_builder.Handlers.Add(newActionHandler);

				return _builder;
			}
		}
	}
}
