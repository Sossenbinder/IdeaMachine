using IdeaMachine.Common.Eventing.Abstractions.Events;
using IdeaMachine.Modules.Reaction.DataTypes.Events;

namespace IdeaMachine.Modules.Reaction.Events.Interface
{
	public interface IReactionEvents
	{
		IDistributedEvent<LikeChange> LikeChange { get; }

		IDistributedEvent<CommentAdded> CommentAdded { get; }
	}
}