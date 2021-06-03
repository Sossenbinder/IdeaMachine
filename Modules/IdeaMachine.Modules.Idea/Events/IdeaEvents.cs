using IdeaMachine.Common.Eventing.Abstractions.Events;
using IdeaMachine.Common.Eventing.Events;
using IdeaMachine.Common.Eventing.MassTransit.Service.Interface;
using IdeaMachine.Modules.Idea.DataTypes.Events;
using IdeaMachine.Modules.Idea.Events.Interface;
using Microsoft.Extensions.Logging;

namespace IdeaMachine.Modules.Idea.Events
{
	public class IdeaEvents : IIdeaEvents
	{
		public IDistributedEvent<IdeaCreated> IdeaCreated { get; }

		public IdeaEvents(
			IMassTransitEventingService massTransitEventingService,
			ILogger<IdeaEvents> logger)
		{
			IdeaCreated = new MtEvent<IdeaCreated>(massTransitEventingService, logger);
		}
	}
}