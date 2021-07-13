using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Common.Eventing.DataTypes;
using IdeaMachine.Common.Eventing.MassTransit.Service.Interface;
using IdeaMachine.Common.SignalR;
using IdeaMachine.Common.SignalR.Service.Interface;
using MassTransit;
using MassTransit.SignalR.Contracts;
using MassTransit.SignalR.Utils;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace IdeaMachine.Common.Eventing.MassTransit.Service
{
	public class MassTransitSignalRBackplaneService : IMassTransitSignalRBackplaneService
	{
		private readonly IBusControl _busControl;

		private readonly IClientTrackingService _clientTrackingService;

		private readonly List<IHubProtocol> _signalRProtocols;

		public MassTransitSignalRBackplaneService(
			IBusControl busControl,
			IClientTrackingService clientTrackingService)
		{
			_busControl = busControl;
			_clientTrackingService = clientTrackingService;

			_signalRProtocols = new List<IHubProtocol>() { new JsonHubProtocol() };
		}

		public async Task RaiseAllSignalREvent<T>(Notification<T> notification, string[]? excludedConnectionIds = null)
		{
			var signalRParams = new
			{
				ExcludedConnectionIds = excludedConnectionIds,
				Messages = CreateProtocolDict(notification)
			};

			await _busControl.Publish<All<SignalRHub>>(signalRParams);
		}

		public async Task RaiseConnectionSignalREvent<T>(string connectionId, Notification<T> notification)
		{
			var signalRParams = new
			{
				ConnectionId = connectionId,
				Messages = CreateProtocolDict(notification)
			};

			await _busControl.Publish<Connection<SignalRHub>>(signalRParams);
		}

		public async Task RaiseGroupSignalREvent<T>(string groupName, Notification<T> notification, string[]? excludedConnectionIds = null)
		{
			var signalRParams = new
			{
				GroupName = groupName,
				ExcludedConnectionIds = excludedConnectionIds,
				Messages = CreateProtocolDict(notification)
			};

			await _busControl.Publish<Group<SignalRHub>>(signalRParams);
		}

		public async Task RaiseUserSignalREvent<T>(Guid userId, Notification<T> notification)
		{
			var signalRParams = new
			{
				UserId = userId.ToString(),
				Messages = CreateProtocolDict(notification)
			};

			await _busControl.Publish<User<SignalRHub>>(signalRParams);
		}

		private IReadOnlyDictionary<string, byte[]> CreateProtocolDict<T>(Notification<T> notification)
		{
			return _signalRProtocols.ToProtocolDictionary(
				notification.NotificationType.ToString(),
				GetPayload(notification));
		}

		private static object[] GetPayload<T>(Notification<T> notification)
		{
			var payload = new object[]
			{
				new
				{
					Operation = notification.Operation,
					Payload = notification.Payload,
				}
			};

			return payload;
		}
	}
}