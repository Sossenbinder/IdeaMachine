using IdeaMachine.Common.Eventing.Abstractions.Events;
using IdeaMachine.Modules.Reaction.DataTypes.Events;
using IdeaMachine.Modules.Reaction.Events.Interface;

namespace IdeaMachine.Modules.Reaction.Events
{
	public class ReactionEvents : IReactionEvents
	{
		public IDistributedEvent<LikeChange> LikeChange { get; }

		public ReactionEvents(IDistributedEvent<LikeChange> likeChange)
		{
			LikeChange = likeChange;
		}
	}
}