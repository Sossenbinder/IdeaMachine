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
using MassTransit.ConsumeConfigurators;
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

		private readonly Action<IReceiveEndpointConfigurator>? _customConfigurator;
		private readonly Action<IInstanceConfigurator<IConsumer<TEventArgs>>>? _instanceConfigurator;

		private readonly ConcurrentHashSet<Func<Fault<TEventArgs>, Task>> _faultHandlers = new();

		private readonly object _registrationLock = new();

		private HostReceiveEndpointHandle? _regularEndpointHandle;

		private HostReceiveEndpointHandle? _faultEndpointHandle;

		public MtEvent(
			IMassTransitEventingService massTransitEventingService,
			IQueueNameFactory queueNameFactory,
			ILogger logger,
			Action<IReceiveEndpointConfigurator>? customConfigurator = null,
			Action<IInstanceConfigurator<IConsumer<TEventArgs>>>? instanceConfigurator = null)
			: this(typeof(TEventArgs).Name, massTransitEventingService, queueNameFactory, logger, customConfigurator)
		{
		}

		public MtEvent(
			string queueName,
			IMassTransitEventingService massTransitEventingService,
			IQueueNameFactory queueNameFactory,
			ILogger logger,
			Action<IReceiveEndpointConfigurator>? customConfigurator = null,
			Action<IInstanceConfigurator<IConsumer<TEventArgs>>>? instanceConfigurator = null)
		{
			_queueName = queueName;
			_massTransitEventingService = massTransitEventingService;
			_queueNameFactory = queueNameFactory;
			_logger = logger;
			_customConfigurator = customConfigurator;
			_instanceConfigurator = instanceConfigurator;
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

				_regularEndpointHandle = _massTransitEventingService.RegisterInstanceConsumer(_queueNameFactory.GetRegularQueueName(_queueName), this, _customConfigurator, _instanceConfigurator);
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
				if (_faultEndpointHandle is not null)
				{
					return disposableAction;
				}

				_faultEndpointHandle = _massTransitEventingService.RegisterInstanceConsumer(_queueNameFactory.GetFaultQueueName(_queueName), this, _customConfigurator);
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

		public async Task Raise(TEventArgs eventArgs)
		{
			await WaitForHandles();
			await _massTransitEventingService.RaiseEvent(eventArgs);
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

		public Task WaitForHandles() => Task.WhenAll(_regularEndpointHandle?.Ready ?? Task.CompletedTask, _faultEndpointHandle?.Ready ?? Task.CompletedTask);
	}
}