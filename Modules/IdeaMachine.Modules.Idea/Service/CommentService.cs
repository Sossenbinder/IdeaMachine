using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Idea.DataTypes.Events;
using IdeaMachine.Modules.Idea.DataTypes.Model;
using IdeaMachine.Modules.Idea.Events.Interface;
using IdeaMachine.Modules.Idea.Repository.Interface;
using IdeaMachine.Modules.Idea.Service.Interface;
using IdeaMachine.ModulesServiceBase;
using Microsoft.Extensions.Logging;

namespace IdeaMachine.Modules.Idea.Service
{
    public class CommentService : ServiceBase, ICommentService
    {
	    private readonly ICommentRepository _commentRepository;

	    public CommentService(
			ILogger<CommentService> logger,
			ICommentRepository commentRepository,
			IIdeaEvents ideaEvents)
			: base(logger)
	    {
		    _commentRepository = commentRepository;
		    RegisterEventHandler(ideaEvents.CommentAdded, OnCommentAdded);
	    }

		private async Task OnCommentAdded(CommentAdded commentAddedArgs)
		{
			await _commentRepository.Add(commentAddedArgs.Comment.ToEntity());
		}

		public async Task<ServiceResponse<List<CommentModel>>> GetComments(int ideaId)
		{
			var comments = await _commentRepository.Get(entity => entity.IdeaId == ideaId);

			return ServiceResponse.Success(comments.Select(x => x.ToModel()).ToList());
		}
    }
}
