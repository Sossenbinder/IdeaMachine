using IdeaMachine.Common.Eventing.Abstractions.Events;
using IdeaMachine.Common.Eventing.Helper;
using IdeaMachine.Modules.Idea.DataTypes.Events;
using IdeaMachine.Modules.Idea.Events.Interface;

namespace IdeaMachine.Modules.Idea.Events
{
	public class IdeaEvents : IIdeaEvents
	{
		public IDistributedEvent<IdeaCreated> IdeaCreated { get; }

		public IdeaEvents(MassTransitEventFactory eventFactory)
		{
			IdeaCreated = eventFactory.CreateDistinct<IdeaCreated>();
		}
	}
}