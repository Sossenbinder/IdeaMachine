using System;

namespace IdeaMachine.Modules.Reaction.DataTypes.Events
{
	public record LikeChange(Guid UserId, int IdeaId, LikeState LikeState);
}