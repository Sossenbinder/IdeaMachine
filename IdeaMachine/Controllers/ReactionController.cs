using System.Threading.Tasks;
using IdeaMachine.DataTypes.UiModels.Reaction;
using IdeaMachine.Modules.Reaction.DataTypes.Events;
using IdeaMachine.Modules.Reaction.Events.Interface;
using IdeaMachine.Modules.Session.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachine.Controllers
{
	[Route("Reaction")]
	public class ReactionController : IdentityControllerBase
	{
		private readonly IReactionEvents _reactionEvents;

		public ReactionController(
			ISessionService sessionService,
			IReactionEvents reactionEvents)
			: base(sessionService)
		{
			_reactionEvents = reactionEvents;
		}

		[HttpPost]
		[Route("ModifyLike")]
		public async Task Like([FromBody] ModifyLikeUiModel modifyLikeUiModel)
		{
			var (ideaId, likeState) = modifyLikeUiModel;

			await _reactionEvents.LikeChange.Raise(new LikeChange(UserId, ideaId, likeState));
		}
	}
}