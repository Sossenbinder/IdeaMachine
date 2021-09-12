using IdeaMachine.Common.Eventing.Abstractions.Events;
using IdeaMachine.Modules.Reaction.DataTypes.Events;
using IdeaMachine.Modules.Reaction.Events.Interface;

namespace IdeaMachine.Modules.Reaction.Events
{
	public class ReactionEvents : IReactionEvents
	{
		public IDistributedEvent<LikeChange> LikeChange { get; }

		public IDistributedEvent<CommentAdded> CommentAdded { get; }

		public ReactionEvents(
			IDistributedEvent<LikeChange> likeChange,
			IDistributedEvent<CommentAdded> commentAdded)
		{
			LikeChange = likeChange;
			CommentAdded = commentAdded;
		}
	}
}