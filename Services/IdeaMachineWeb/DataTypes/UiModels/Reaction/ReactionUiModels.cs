using System;
using IdeaMachine.Modules.Reaction.Abstractions.DataTypes;
using IdeaMachine.Modules.Reaction.DataTypes.Events;

namespace IdeaMachineWeb.DataTypes.UiModels.Reaction
{
	public record ModifyLikeUiModel(int IdeaId, LikeState LikeState);

	public record RespondUiModel(int IdeaId, string Response)
	{
		public ResponseSent AsDto(Guid userId) => new(userId, IdeaId, Response);
	}
}