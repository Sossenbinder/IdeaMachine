namespace IdeaMachine.Common.Eventing.Abstractions.Options
{
	public class RabbitMqOptions
	{
		public string BrokerAddress { get; set; } = default!;

		public string UserName { get; set; } = default!;

		public string Password { get; set; } = default!;
	}
}