using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Disposable;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Common.Core.Extensions.Async;
using IdeaMachine.Common.Core.Utils.Collections;
using IdeaMachine.Common.Core.Utils.Concurrency;
using IdeaMachine.Common.Core.Utils.Tasks;
using IdeaMachine.Common.Eventing.Abstractions.Events;
using IdeaMachine.Common.Eventing.DataTypes;
using IdeaMachine.Common.Eventing.MassTransit.Service.Interface;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace IdeaMachine.Common.Eventing.Events
{
	/// <summary>
	/// Event hub which registers itself as IConsumer and then distributes events based on service-local registrations.
	/// This way, there is a more fine-grained control for disposal and lifetimes, as MT does not support
	/// connecting / disconnecting on the fly
	/// </summary>
	/// <typeparam name="TEventArgs"></typeparam>
	public class MtEvent<TEventArgs> : EventBase<TEventArgs>, IDistributedEvent<TEventArgs>, IConsumer<TEventArgs>, IConsumer<Fault<TEventArgs>> where TEventArgs : class
	{
		private readonly string _queueName;

		private readonly IMassTransitEventingService _massTransitEventingService;

		private readonly ILogger _logger;

		private readonly ConcurrentHashSet<Func<Fault<TEventArgs>, Task>> _faultHandlers = new();

		private readonly object _registrationLock = new();

		private InterlockedBool _consumersRegistered;

		public MtEvent(
			IMassTransitEventingService massTransitEventingService,
			ILogger logger)
			: this(typeof(TEventArgs).Name, massTransitEventingService, logger)
		{
		}

		public MtEvent(
			MemberInfo t,
			IMassTransitEventingService massTransitEventingService,
			ILogger logger)
			: this(t.Name, massTransitEventingService, logger)
		{
		}

		public MtEvent(
			string queueName,
			IMassTransitEventingService massTransitEventingService,
			ILogger logger)
		{
			_queueName = queueName;
			_massTransitEventingService = massTransitEventingService;
			_logger = logger;
		}

		public override DisposableAction Register(Func<TEventArgs, Task> handler)
		{
			var disposableAction = base.Register(handler);

			if (_consumersRegistered)
			{
				return disposableAction;
			}

			_consumersRegistered = true;

			_massTransitEventingService.RegisterConsumer<IConsumer<TEventArgs>>(_queueName, this);

			return disposableAction;
		}

		/// <summary>
		/// Implementation of the IConsumer interface, this is where the event comes in
		/// </summary>
		public async Task Consume(ConsumeContext<TEventArgs> context)
		{
			try
			{
				var message = context.Message;
				await Handlers.ToArray().ParallelAsync(handler => handler(message));
			}
			catch (Exception e)
			{
				_logger.LogException(e, e.Message);

				// It is important to throw here, as a consumer registration will raise an {queueName}_error event
				// when an exception is passed to the calling MassTransit code!
				throw;
			}
		}

		/// <summary>
		/// Entry point for errors raised through exceptions in the original event
		/// </summary>
		public async Task Consume(ConsumeContext<Fault<TEventArgs>> context)
		{
			try
			{
				await _faultHandlers.ToArray().ParallelAsync(handler => handler(context.Message));
			}
			catch (Exception e)
			{
				// Log the error here - But don't rethrow. This was the final chance to compensate an erroneous state
				_logger.LogException(e, e.Message);
			}
		}

		public void RaiseFireAndForget(TEventArgs eventArgs)
		{
			_ = FireAndForgetTask.Run(() => _massTransitEventingService.RaiseEvent(eventArgs), _logger);
		}

		/// <summary>
		/// Register a fault handler to compensate for failures on this event
		/// </summary>
		public DisposableAction RegisterForErrors(Func<Fault<TEventArgs>, Task> faultHandler)
		{
			_faultHandlers.Add(faultHandler);

			lock (_registrationLock)
			{
				_massTransitEventingService.RegisterConsumer<IConsumer<Fault<TEventArgs>>>(_queueName, this, QueueType.Error);
			}

			return new DisposableAction(() => _faultHandlers.Remove(faultHandler));
		}

		public Task Raise(TEventArgs eventArgs)
		{
			return _massTransitEventingService.RaiseEvent(eventArgs);
		}
	}
}