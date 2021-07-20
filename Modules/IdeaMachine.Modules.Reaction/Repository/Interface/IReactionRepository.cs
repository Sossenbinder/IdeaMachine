using System;
using System.Threading.Tasks;
using IdeaMachine.Modules.Reaction.Abstractions.DataTypes;

namespace IdeaMachine.Modules.Reaction.Repository.Interface
{
	public interface IReactionRepository
	{
		Task<bool> PutReaction(Guid userId, int ideaId, LikeState likeState);

		Task<int> GetTotalLikeCount(int ideaId);

		Task<LikeState> GetLikeState(int ideaId, Guid userId);
	}
}