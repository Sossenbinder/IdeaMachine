using System;
using IdeaMachine.Modules.Reaction.Abstractions.DataTypes;
using IdeaMachine.Modules.Reaction.DataTypes.Models;

namespace IdeaMachine.Modules.Reaction.DataTypes.Events
{
	public record LikeChange(Guid UserId, int IdeaId, LikeState LikeState);

	public record CommentAdded(CommentModel Comment);
}