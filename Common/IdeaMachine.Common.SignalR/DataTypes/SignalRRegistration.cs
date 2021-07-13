namespace IdeaMachine.Common.SignalR.DataTypes
{
	public class SignalRRegistration
	{
		public string ConnectionId { get; set; }

		public bool Invalidated { get; set; }

		public SignalRRegistration(string connectionId) => (ConnectionId, Invalidated) = (connectionId, false);
	}
}