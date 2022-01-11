using System.Threading.Tasks;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.Modules.Reaction.DataTypes.Events;
using IdeaMachine.Modules.Reaction.Events.Interface;
using IdeaMachineWeb.DataTypes.UiModels.Reaction;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachineWeb.Controllers
{
	[Route("Reaction")]
	public class ReactionController : IdentityControllerBase
	{
		private readonly IReactionEvents _reactionEvents;

		private readonly IPublishEndpoint _publishEndpoint;

		public ReactionController(
			IReactionEvents reactionEvents,
			IPublishEndpoint publishEndpoint)
		{
			_reactionEvents = reactionEvents;
			_publishEndpoint = publishEndpoint;
		}

		[HttpPost]
		[Route("ModifyLike")]
		public async Task<JsonResponse> Like([FromBody] ModifyLikeUiModel modifyLikeUiModel)
		{
			var (ideaId, likeState) = modifyLikeUiModel;

			await _reactionEvents.LikeChange.Raise(new LikeChange(UserId, ideaId, likeState));

			return JsonResponse.Success();
		}

		[HttpPost]
		[Route("Share")]
		public async Task<JsonResponse> Respond([FromBody] RespondUiModel responseModel)
		{
			await _publishEndpoint.Publish(responseModel.AsDto(UserId));

			return JsonResponse.Success();
		}
	}
}