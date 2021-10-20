using IdeaMachine.Modules.Reaction.Abstractions.DataTypes;

namespace IdeaMachineWeb.DataTypes.UiModels.Reaction
{
	public record ModifyLikeUiModel(int IdeaId, LikeState LikeState);
}