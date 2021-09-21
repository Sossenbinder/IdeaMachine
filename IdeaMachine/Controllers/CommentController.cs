using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.DataTypes.UiModels.Comment;
using IdeaMachine.Extensions;
using IdeaMachine.Modules.Idea.DataTypes.Events;
using IdeaMachine.Modules.Idea.DataTypes.Model;
using IdeaMachine.Modules.Idea.Events.Interface;
using IdeaMachine.Modules.Idea.Service.Interface;
using IdeaMachine.Modules.Session.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachine.Controllers
{
	[ApiController]
	[Authorize]
	[Route("[controller]")]
    public class CommentController : IdentityControllerBase
    {
	    private readonly IIdeaEvents _ideaEvents;

	    private readonly ICommentService _commentService;

	    public CommentController(
		    ISessionService sessionService,
		    IIdeaEvents ideaEvents, 
		    ICommentService commentService)
		    : base(sessionService)
	    {
		    _ideaEvents = ideaEvents;
		    _commentService = commentService;
	    }

	    [HttpPost]
	    public async Task<JsonResponse> Add(CommentUiModel commentModel)
	    {
		    var (_, _, ideaId, comment, _) = commentModel;

		    var model = new CommentModel()
		    {
			    Comment = comment,
			    CommenterId = UserId,
			    IdeaId = ideaId,
		    };

		    await _ideaEvents.CommentAdded.Raise(new CommentAdded(model));

		    return JsonResponse.Success();
	    }

	    [HttpGet]
	    public async Task<JsonDataResponse<List<CommentUiModel>>> GetComments(GetCommentUiModel getModel)
	    {
		    var comments = await _commentService.GetComments(getModel.IdeaId);

		    return comments.ToJsonDataResponse(CommentUiModel.FromModel);
	    }
    }
}
