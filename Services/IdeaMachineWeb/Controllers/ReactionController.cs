using System.Threading.Tasks;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.Modules.Reaction.DataTypes.Events;
using IdeaMachine.Modules.Reaction.Events.Interface;
using IdeaMachine.Modules.Session.Service.Interface;
using IdeaMachineWeb.DataTypes.UiModels.Reaction;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachineWeb.Controllers
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
		public async Task<JsonResponse> Like([FromBody] ModifyLikeUiModel modifyLikeUiModel)
		{
			var (ideaId, likeState) = modifyLikeUiModel;

			await _reactionEvents.LikeChange.Raise(new LikeChange(UserId, ideaId, likeState));

			return JsonResponse.Success();
		}
	}
}