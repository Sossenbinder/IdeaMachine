using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Eventing.DataTypes;

namespace IdeaMachine.Common.Eventing.MassTransit.Service.Interface
{
	public interface IMassTransitSignalRBackplaneService
	{
		Task RaiseAllSignalREvent<T>(Notification<T> notification, string[]? excludedConnectionIds = null);

		Task RaiseConnectionSignalREvent<T>(string connectionId, Notification<T> notification);

		Task RaiseGroupSignalREvent<T>(string groupName, Notification<T> notification, string[]? excludedConnectionIds = null);

		Task RaiseUserSignalREvent<T>(Guid userId, Notification<T> notification);
	}
}