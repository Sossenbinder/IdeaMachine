using IdeaMachine.Common.Eventing.Abstractions.Events;
using IdeaMachine.Modules.Idea.DataTypes.Events;

namespace IdeaMachine.Modules.Idea.Events.Interface
{
	public interface IIdeaEvents
	{
		IDistributedEvent<IdeaCreated> IdeaCreated { get; }

		IDistributedEvent<CommentAdded> CommentAdded { get; }
	}
}