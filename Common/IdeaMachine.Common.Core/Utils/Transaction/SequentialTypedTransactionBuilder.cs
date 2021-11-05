//using System;
//using System.Threading.Tasks;
//using Serilog;

//namespace IdeaMachine.Common.Core.Utils.Transaction
//{
//	public record Void;

//	public record ActionHandler<TIn, TOut>(Func<TIn, Task<TOut>> Action, Func<Task> CompensationAction);

//	/// <summary>
//	/// Executes all actions sequentially, and when an exception occurs, it will rewind the exceptions from that exception on.
//	/// Every action receives the output of the preceding function
//	/// </summary>
//	public class SequentialTypedTransactionBuilder<TIn, TOut, TInPrev>
//	{
//		private ActionHandler<TIn, TInPrev> Handler { get; set; }
//			= new((_) => Task.FromResult(default(TInPrev))!, () => Task.CompletedTask);

//		private SequentialTypedTransactionBuilder<TInPrev, TOut, TIn>? PreviousBuilder { get; set; }

//		public CompensationBuilder<TNextOut> RegisterAction<TNextOut>(Func<TIn, Task<TNextOut>> action)
//		{
//			return new CompensationBuilder<TNextOut>(action, this);
//		}

//		public async Task Execute(TIn input)
//		{
//			try
//			{
//				var output = await Handler.Action(input);

//				if (PreviousBuilder is not null)
//				{
//					await PreviousBuilder.Execute(output);
//				}
//			}
//			catch (Exception ex)
//			{
//				Log.Logger.Error(ex, "Exception occurred");
//				await Compensate();

//				if (PreviousBuilder is not null)
//				{
//					await PreviousBuilder.Compensate();
//				}
//			}
//		}

//		private async Task Compensate()
//		{
//			await Handler.CompensationAction();
//		}

//		public class CompensationBuilder<TNextOut>
//		{
//			private readonly Func<TIn, Task<TNextOut>> _action;

//			private readonly SequentialTypedTransactionBuilder<TIn, TOut, TInPrev> _builder;

//			internal CompensationBuilder(
//				Func<TIn, Task<TNextOut>> action,
//				SequentialTypedTransactionBuilder<TIn, TOut, TInPrev> builder)
//			{
//				_action = action;
//				_builder = builder;
//			}

//			public SequentialTypedTransactionBuilder<TIn, TOut, TNextOut> WithCompensation(Func<Task> compensationAction)
//			{
//				var currentHandler = new ActionHandler<TIn, TNextOut>(_action, compensationAction);

//				var newBuilder = new SequentialTypedTransactionBuilder<TIn, TOut, TNextOut>
//				{
//					Handler = currentHandler,
//					PreviousBuilder = _builder,
//				};

//				return newBuilder;
//			}
//		}
//	}

//	public class Test
//	{
//		public async Task Blub()
//		{
//			var builder1 = new SequentialTypedTransactionBuilder<Void, Void, Void>()
//				.RegisterAction<int>(_ => Task.FromResult(5))
//				.WithCompensation(() => Task.CompletedTask)
//				.Execute(5);

//			var builder = await new SequentialTypedTransactionBuilder<Void, Void, Void>()
//				.RegisterAction(_ => Task.FromResult(new Void()))
//				.WithCompensation(() => Task.CompletedTask)
//				.RegisterAction<int>(_ => Task.FromResult(5))
//				.WithCompensation(() => Task.CompletedTask)
//				.RegisterAction(previousInt => Task.FromResult("hello"))
//				.WithCompensation(() => Task.CompletedTask)
//				.RegisterAction(x =>
//				{
//					Console.WriteLine(x);
//					return Task.FromResult(new Void());
//				})
//				.WithCompensation(() => Task.CompletedTask)
//				.Execute("");
//		}
//	}
//}
