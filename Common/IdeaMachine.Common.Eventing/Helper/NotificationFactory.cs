using IdeaMachine.Common.Eventing.DataTypes;

namespace IdeaMachine.Common.Eventing.Helper
{
	public static class NotificationFactory
	{
		public static Notification<T> Create<T>(T payload, NotificationType notificationType)
			=> CreateNotification(payload, notificationType, Operation.Create);

		public static Notification<T> Update<T>(T payload, NotificationType notificationType)
			=> CreateNotification(payload, notificationType, Operation.Update);

		public static Notification<T> Delete<T>(T payload, NotificationType notificationType)
			=> CreateNotification(payload, notificationType, Operation.Delete);

		private static Notification<T> CreateNotification<T>(
			T payload,
			NotificationType notificationType,
			Operation operation)
		{
			return new()
			{
				Payload = payload,
				NotificationType = notificationType,
				Operation = operation
			};
		}
	}
}