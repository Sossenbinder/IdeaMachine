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
	public class MassTransitNotificationService : INotificationService
	{
		private readonly IPublishEndpoint _publishEndpoint;

		private readonly List<IHubProtocol> _signalRProtocols;

		public MassTransitNotificationService(IPublishEndpoint publishEndpoint)
		{
			_publishEndpoint = publishEndpoint;

			_signalRProtocols = new List<IHubProtocol>() { new JsonHubProtocol() };
		}

		public async Task RaiseForAll<T>(Notification<T> notification, string[]? excludedConnectionIds = null)
		{
			var signalRParams = new
			{
				ExcludedConnectionIds = excludedConnectionIds,
				Messages = CreateProtocolDict(notification)
			};

			await _publishEndpoint.Publish<All<SignalRHub>>(signalRParams);
		}

		public async Task RaiseForGroup<T>(string groupName, Notification<T> notification, string[]? excludedConnectionIds = null)
		{
			var signalRParams = new
			{
				GroupName = groupName,
				ExcludedConnectionIds = excludedConnectionIds,
				Messages = CreateProtocolDict(notification)
			};

			await _publishEndpoint.Publish<Group<SignalRHub>>(signalRParams);
		}

		public async Task RaiseForUser<T>(Guid userId, Notification<T> notification)
		{
			var signalRParams = new
			{
				GroupName = userId,
				Messages = CreateProtocolDict(notification)
			};

			await _publishEndpoint.Publish<Group<SignalRHub>>(signalRParams);
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