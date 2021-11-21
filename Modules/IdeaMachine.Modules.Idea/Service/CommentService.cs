using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Common.Eventing.DataTypes;
using IdeaMachine.Common.Eventing.Helper;
using IdeaMachine.Common.Eventing.MassTransit.Service.Interface;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Model;
using IdeaMachine.Modules.Account.Service.Interface;
using IdeaMachine.Modules.Idea.DataTypes.Events;
using IdeaMachine.Modules.Idea.DataTypes.Model;
using IdeaMachine.Modules.Idea.Events.Interface;
using IdeaMachine.Modules.Idea.Repository.Interface;
using IdeaMachine.Modules.Idea.Service.Interface;
using IdeaMachine.ModulesServiceBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdeaMachine.Modules.Idea.Service
{
    public class CommentService : ServiceBase, ICommentService
    {
	    private readonly ICommentRepository _commentRepository;

	    private readonly IIdeaRepository _ideaRepository;

	    private readonly ISignalRService _signalRService;

	    private readonly IAccountService _accountService;

		public CommentService(
			ILogger<CommentService> logger,
			ICommentRepository commentRepository,
			IIdeaRepository ideaRepository,
			IIdeaEvents ideaEvents,
			ISignalRService signalRService, 
			IAccountService accountService)
			: base(logger)
	    {
		    _commentRepository = commentRepository;
		    _ideaRepository = ideaRepository;
		    _signalRService = signalRService;
		    _accountService = accountService;
		    RegisterEventHandler(ideaEvents.CommentAdded, OnCommentAdded);
	    }

		private async Task OnCommentAdded(CommentAdded commentAddedArgs)
		{
			var comment = commentAddedArgs.Comment;
			var entity = comment.ToEntity();

			if (!await _commentRepository.Add(entity))
			{
				Logger.LogError("Something went wrong while trying to add comment \"{0}\" of user {1}.", comment.Comment, comment.CommenterId);
				throw new DbUpdateException();
			}

			var ownerId = await _ideaRepository.GetOwner(commentAddedArgs.Comment.IdeaId);

			if (ownerId is not null)
			{
				var accountNameResponse = await _accountService.GetAccountName(new GetAccountNameRequest(comment.CommenterId));
				comment.CommenterName = accountNameResponse.PayloadOrNull ?? "Unknown";

				comment.TimeStamp = entity.CreationDate;
				comment.IdeaId = entity.IdeaId;
				await _signalRService.RaiseGroupSignalREvent(ownerId.Value.ToString(), NotificationFactory.Create(comment.ToUiModel(), NotificationType.Comment));
			}
		}

		public async Task<ServiceResponse<List<CommentModel>>> GetComments(int ideaId)
		{
			var comments = await _commentRepository.GetComments(ideaId);

			return ServiceResponse.Success(comments.Select(x => x.ToModel()).ToList());
		}
    }
}
