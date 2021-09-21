using IdeaMachine.Common.Eventing.Abstractions.Events;
using IdeaMachine.Modules.Idea.DataTypes.Events;
using IdeaMachine.Modules.Idea.Events.Interface;

namespace IdeaMachine.Modules.Idea.Events
{
	public class IdeaEvents : IIdeaEvents
	{
		public IDistributedEvent<IdeaCreated> IdeaCreated { get; }

		public IDistributedEvent<CommentAdded> CommentAdded { get; }

		public IdeaEvents(
			IDistributedEvent<IdeaCreated> ideaCreated, 
			IDistributedEvent<CommentAdded> commentAdded)
		{
			IdeaCreated = ideaCreated;
			CommentAdded = commentAdded;
		}
	}
}