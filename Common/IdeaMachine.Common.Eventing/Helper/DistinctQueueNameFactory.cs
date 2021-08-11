using System.Net;
using IdeaMachine.Common.Eventing.Helper.Interface;

namespace IdeaMachine.Common.Eventing.Helper
{
	/// <summary>
	/// To be used when the endpoint should always receive an event (It will receive its own queue)
	/// </summary>
	public class DistinctQueueNameFactory : IQueueNameFactory
	{
		public string GetRegularQueueName(string queueName) => $"{queueName}_{Dns.GetHostName()}";

		public string GetFaultQueueName(string queueName) => $"{GetRegularQueueName(queueName)}_error";
	}
}