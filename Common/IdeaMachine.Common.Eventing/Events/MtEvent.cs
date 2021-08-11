using System;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Disposable;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Common.Core.Extensions.Async;
using IdeaMachine.Common.Core.Utils.Collections;
using IdeaMachine.Common.Eventing.Abstractions.Events;
using IdeaMachine.Common.Eventing.Helper.Interface;
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
	public class MtEvent<TEventArgs> :
		EventBase<TEventArgs>,
		IDistributedEvent<TEventArgs>,
		IConsumer<TEventArgs>,
		IConsumer<Fault<TEventArgs>>,
		IDisposable,
		IAsyncDisposable
		where TEventArgs : class
	{
		private readonly string _queueName;

		private readonly IMassTransitEventingService _massTransitEventingService;

		private readonly IQueueNameFactory _queueNameFactory;

		private readonly ILogger _logger;

		private readonly ConcurrentHashSet<Func<Fault<TEventArgs>, Task>> _faultHandlers = new();

		private readonly object _registrationLock = new();

		private HostReceiveEndpointHandle? _regularEndpointHandle;

		private HostReceiveEndpointHandle? _faultEndpointHandle;

		public MtEvent(
			IMassTransitEventingService massTransitEventingService,
			IQueueNameFactory queueNameFactory,
			ILogger logger)
			: this(typeof(TEventArgs).Name, massTransitEventingService, queueNameFactory, logger)
		{
			_queueNameFactory = queueNameFactory;
		}

		public MtEvent(
			string queueName,
			IMassTransitEventingService massTransitEventingService,
			IQueueNameFactory queueNameFactory,
			ILogger logger)
		{
			_queueName = queueName;
			_massTransitEventingService = massTransitEventingService;
			_queueNameFactory = queueNameFactory;
			_logger = logger;
		}

		public override DisposableAction Register(Func<TEventArgs, Task> handler)
		{
			var disposableAction = base.Register(handler);

			lock (_registrationLock)
			{
				if (_regularEndpointHandle is not null)
				{
					return disposableAction;
				}

				_regularEndpointHandle = _massTransitEventingService.RegisterConsumer<IConsumer<TEventArgs>>(_queueNameFactory.GetRegularQueueName(_queueName), this);
			}

			return disposableAction;
		}

		/// <summary>
		/// Register a fault handler to compensate for failures on this event
		/// </summary>
		public DisposableAction RegisterForErrors(Func<Fault<TEventArgs>, Task> faultHandler)
		{
			_faultHandlers.Add(faultHandler);

			var disposableAction = new DisposableAction(() => _faultHandlers.Remove(faultHandler));

			lock (_registrationLock)
			{
				if (_regularEndpointHandle is not null)
				{
					return disposableAction;
				}

				_faultEndpointHandle = _massTransitEventingService.RegisterConsumer<IConsumer<Fault<TEventArgs>>>(_queueNameFactory.GetFaultQueueName(_queueName), this);
			}

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
				var message = context.Message;
				await _faultHandlers.ToArray().ParallelAsync(handler => handler(message));
			}
			catch (Exception e)
			{
				_logger.LogException(e, e.Message);
				throw;
			}
		}

		public Task Raise(TEventArgs eventArgs)
		{
			// ReSharper disable once InconsistentlySynchronizedField
			return _massTransitEventingService.RaiseEvent(eventArgs);
		}

		public void Dispose()
		{
			TaskCompat.CallSync(async () => await DisposeAsync());
		}

		public async ValueTask DisposeAsync()
		{
			static Task StopIfNecessary(HostReceiveEndpointHandle? handle) => handle?.StopAsync() ?? Task.CompletedTask;

			// ReSharper disable once InconsistentlySynchronizedField
			await Task.WhenAll(StopIfNecessary(_regularEndpointHandle), StopIfNecessary(_faultEndpointHandle));
			GC.SuppressFinalize(this);
		}
	}
}