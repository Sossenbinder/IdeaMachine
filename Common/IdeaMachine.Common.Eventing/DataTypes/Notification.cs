namespace IdeaMachine.Common.Eventing.DataTypes
{
	public class Notification<T>
	{
		public Operation Operation { get; set; }

		public NotificationType NotificationType { get; set; }

		public T? Payload { get; set; }
	}
}