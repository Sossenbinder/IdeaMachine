using IdeaMachine.Common.Eventing.Helper.Interface;

namespace IdeaMachine.Common.Eventing.Helper
{
	/// <summary>
	/// To be used when the endpoint should participate in queue in round robin fashion
	/// (E.g. multiple endpoints register on one queue -> Only one of them receives the event)
	/// </summary>
	public class SharedQueueNameFactory : IQueueNameFactory
	{
		public string GetRegularQueueName(string queueName) => queueName;

		public string GetFaultQueueName(string queueName) => $"{queueName}_error";
	}
}