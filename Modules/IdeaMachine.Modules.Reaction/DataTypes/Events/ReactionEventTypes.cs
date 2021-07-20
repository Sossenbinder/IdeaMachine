using System;
using IdeaMachine.Modules.Reaction.Abstractions.DataTypes;

namespace IdeaMachine.Modules.Reaction.DataTypes.Events
{
	public record LikeChange(Guid UserId, int IdeaId, LikeState LikeState);
}