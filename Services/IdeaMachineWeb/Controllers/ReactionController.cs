using System.Threading.Tasks;
using IdeaMachine.Modules.Reaction.DataTypes.Events;
using IdeaMachineWeb.DataTypes.UiModels.Reaction;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachineWeb.Controllers
{
	[Route("Reaction")]
	public class ReactionController : IdentityControllerBase
	{
		private readonly IPublishEndpoint _publishEndpoint;

		public ReactionController(IPublishEndpoint publishEndpoint) => _publishEndpoint = publishEndpoint;

		[HttpPost]
		[Route("ModifyLike")]
		public async Task<IActionResult> Like([FromBody] ModifyLikeUiModel modifyLikeUiModel)
		{
			var (ideaId, likeState) = modifyLikeUiModel;

			await _publishEndpoint.Publish(new LikeChange(UserId, ideaId, likeState));

			return Ok();
		}

		[HttpPost]
		[Route("Share")]
		public async Task<IActionResult> Respond([FromBody] RespondUiModel responseModel)
		{
			await _publishEndpoint.Publish(responseModel.AsDto(UserId));

			return Ok();
		}
	}
}