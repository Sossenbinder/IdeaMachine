using System;
using IdeaMachine.Common.Eventing.DataTypes;

namespace IdeaMachine.Common.Eventing.Helper.Interface
{
	public interface IQueueNameFactory
	{
		string GetQueueName(string queueName, QueueType queueType)
		{
			return queueType switch
			{
				QueueType.Error => GetFaultQueueName(queueName),
				QueueType.Regular => GetRegularQueueName(queueName),
				_ => throw new ArgumentOutOfRangeException(nameof(queueType), queueType, null)
			};
		}

		string GetRegularQueueName(string queueName);

		string GetFaultQueueName(string queueName);
	}
}