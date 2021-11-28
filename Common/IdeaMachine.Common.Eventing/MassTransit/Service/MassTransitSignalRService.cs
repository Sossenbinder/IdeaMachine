using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Common.Eventing.DataTypes;
using IdeaMachine.Common.Eventing.MassTransit.Service.Interface;
using IdeaMachine.Common.SignalR;
using MassTransit;
using MassTransit.SignalR.Contracts;
using MassTransit.SignalR.Utils;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace IdeaMachine.Common.Eventing.MassTransit.Service
{
	public class MassTransitSignalRService : ISignalRService
	{
		private readonly IPublishEndpoint _publishEndpoint;

		private readonly List<IHubProtocol> _signalRProtocols;

		public MassTransitSignalRService(IPublishEndpoint publishEndpoint)
		{
			_publishEndpoint = publishEndpoint;

			_signalRProtocols = new List<IHubProtocol>() { new JsonHubProtocol() };
		}

		public async Task RaiseAllSignalREvent<T>(Notification<T> notification, string[]? excludedConnectionIds = null)
		{
			var signalRParams = new
			{
				ExcludedConnectionIds = excludedConnectionIds,
				Messages = CreateProtocolDict(notification)
			};

			await _publishEndpoint.Publish<All<SignalRHub>>(signalRParams);
		}

		public async Task RaiseConnectionSignalREvent<T>(string connectionId, Notification<T> notification)
		{
			var signalRParams = new
			{
				ConnectionId = connectionId,
				Messages = CreateProtocolDict(notification)
			};

			await _publishEndpoint.Publish<Connection<SignalRHub>>(signalRParams);
		}

		public async Task RaiseGroupSignalREvent<T>(string groupName, Notification<T> notification, string[]? excludedConnectionIds = null)
		{
			var signalRParams = new
			{
				GroupName = groupName,
				ExcludedConnectionIds = excludedConnectionIds,
				Messages = CreateProtocolDict(notification)
			};

			await _publishEndpoint.Publish<Group<SignalRHub>>(signalRParams);
		}

		public async Task RaiseUserSignalREvent<T>(Guid userId, Notification<T> notification)
		{
			var signalRParams = new
			{
				UserId = userId,
				Messages = CreateProtocolDict(notification)
			};

			await _publishEndpoint.Publish<User<SignalRHub>>(signalRParams);
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
					notification.Operation, 
					notification.Payload,
				}
			};

			return payload;
		}
	}
}