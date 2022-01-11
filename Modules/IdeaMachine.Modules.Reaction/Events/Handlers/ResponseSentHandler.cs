using System;
using System.Threading.Tasks;
using IdeaMachine.Modules.Reaction.DataTypes.Events;
using MassTransit;

namespace IdeaMachine.Modules.Reaction.Events.Handlers
{
	public class ResponseSentHandler : IConsumer<ResponseSent>
	{
		public async Task Consume(ConsumeContext<ResponseSent> context)
		{
		}
	}
}