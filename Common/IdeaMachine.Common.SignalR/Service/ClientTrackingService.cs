using System;
using System.Collections.Generic;
using IdeaMachine.Common.SignalR.DataTypes;
using IdeaMachine.Common.SignalR.Service.Interface;

namespace IdeaMachine.Common.SignalR.Service
{
	public class ClientTrackingService : IClientTrackingService
	{
		public readonly Dictionary<Guid, SignalRRegistration> _registrations;

		public ClientTrackingService()
		{
			_registrations = new();
		}

		public SignalRRegistration AddRegistration(Guid userId, string connectionId)
		{
			if (_registrations.TryGetValue(userId, out var connection))
			{
				return connection;
			}

			var registration = new SignalRRegistration(connectionId);
			_registrations.Add(userId, registration);

			return registration;
		}

		public string? GetConnectionId(Guid userId)
		{
			return _registrations.TryGetValue(userId, out var value) ? value.ConnectionId : null;
		}

		public void InvalidateRegistration(Guid userId)
		{
			if (!_registrations.TryGetValue(userId, out var registration))
			{
				return;
			}

			registration.Invalidated = true;

			_registrations.Remove(userId);
		}
	}
}