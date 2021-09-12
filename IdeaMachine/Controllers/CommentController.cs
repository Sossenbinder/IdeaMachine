using System.Threading.Tasks;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.DataTypes.UiModels.Comment;
using IdeaMachine.Modules.Reaction.DataTypes.Events;
using IdeaMachine.Modules.Reaction.DataTypes.Models;
using IdeaMachine.Modules.Reaction.Events.Interface;
using IdeaMachine.Modules.Session.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachine.Controllers
{
	[ApiController]
	[Route("[controller]")]
    public class CommentController : IdentityControllerBase
    {
	    private readonly IReactionEvents _reactionEvents;

	    public CommentController(
		    ISessionService sessionService,
		    IReactionEvents reactionEvents)
		    : base(sessionService)
	    {
		    _reactionEvents = reactionEvents;
	    }

	    [HttpPost]
	    public async Task<JsonResponse> Add(CommentUiModel comment)
	    {
		    var (ideaId, s) = comment;
		    var model = new CommentModel()
		    {
			    Comment = s,
			    CommenterId = UserId,
			    IdeaId = ideaId,
		    };

		    await _reactionEvents.CommentAdded.Raise(new CommentAdded(model));

		    return JsonResponse.Success();
	    }
    }
}
