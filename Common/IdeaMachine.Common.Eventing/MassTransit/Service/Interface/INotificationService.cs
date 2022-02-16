using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Eventing.DataTypes;

namespace IdeaMachine.Common.Eventing.MassTransit.Service.Interface
{
	public interface INotificationService
	{
		Task RaiseForAll<T>(Notification<T> notification, string[]? excludedConnectionIds = null);

		Task RaiseForGroup<T>(string groupName, Notification<T> notification, string[]? excludedConnectionIds = null);

		Task RaiseForUser<T>(Guid userId, Notification<T> notification);
	}
}