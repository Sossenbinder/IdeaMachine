using System;
using System.Threading.Tasks;
using IdeaMachine.Modules.Reaction.DataTypes;

namespace IdeaMachine.Modules.Reaction.Repository.Interface
{
	public interface IReactionRepository
	{
		Task<bool> PutReaction(Guid userId, int ideaId, LikeState likeState);
	}
}