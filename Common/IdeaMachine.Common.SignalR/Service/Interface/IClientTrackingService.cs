using System;
using IdeaMachine.Common.SignalR.DataTypes;

namespace IdeaMachine.Common.SignalR.Service.Interface
{
	public interface IClientTrackingService
	{
		SignalRRegistration AddRegistration(Guid userId, string connectionId);

		string? GetConnectionId(Guid userId);

		void InvalidateRegistration(Guid userId);
	}
}