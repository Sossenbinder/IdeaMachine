using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Extensions.Async;
using IdeaMachine.Modules.Idea.Abstractions.DataTypes.Model;
using IdeaMachine.Modules.Reaction.Abstractions.DataTypes;
using IdeaMachine.Modules.Reaction.Repository.Interface;
using IdeaMachine.Modules.Reaction.Service.Interface;

namespace IdeaMachine.Modules.Reaction.Service
{
	public class ReactionRetrievalService : IReactionRetrievalService
	{
		private readonly IReactionRepository _reactionRepository;

		public ReactionRetrievalService(IReactionRepository reactionRepository)
		{
			_reactionRepository = reactionRepository;
		}

		public async Task<IdeaReactionMetaData> GetIdeaReactionMetaData(int ideaId, Guid userId)
		{
			var (totalLikeCount, personalLikeState) = await (
				_reactionRepository.GetTotalLikeCount(ideaId),
				_reactionRepository.GetLikeState(ideaId, userId));

			return new IdeaReactionMetaData(totalLikeCount, personalLikeState ?? LikeState.Neutral);
		}
	}
}