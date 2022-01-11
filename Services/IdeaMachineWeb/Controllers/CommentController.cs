﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.Modules.Idea.DataTypes.Events;
using IdeaMachine.Modules.Idea.DataTypes.Model;
using IdeaMachine.Modules.Idea.DataTypes.UiModel;
using IdeaMachine.Modules.Idea.Events.Interface;
using IdeaMachine.Modules.Idea.Service.Interface;
using IdeaMachine.Modules.Session.Service.Interface;
using IdeaMachineWeb.DataTypes.UiModels.Comment;
using IdeaMachineWeb.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachineWeb.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CommentController : IdentityControllerBase
	{
		private readonly IIdeaEvents _ideaEvents;

		private readonly ICommentService _commentService;

		public CommentController(
			IIdeaEvents ideaEvents,
			ICommentService commentService)
		{
			_ideaEvents = ideaEvents;
			_commentService = commentService;
		}

		[HttpPost]
		[Authorize]
		[Route("Add")]
		public async Task<JsonResponse> Add([FromBody] AddCommentUiModel commentModel)
		{
			var model = new CommentModel()
			{
				Comment = commentModel.Comment,
				CommenterId = UserId,
				IdeaId = commentModel.IdeaId,
				TimeStamp = DateTime.UtcNow,
			};

			await _ideaEvents.CommentAdded.Raise(new CommentAdded(model));

			return JsonResponse.Success();
		}

		[HttpPost]
		[Route("GetComments")]
		public async Task<JsonDataResponse<List<CommentUiModel>>> GetComments([FromBody] GetCommentUiModel commentUiModel)
		{
			var comments = await _commentService.GetComments(commentUiModel.IdeaId);

			return comments.ToJsonDataResponse(CommentUiModel.FromModel);
		}
	}
}