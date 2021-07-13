using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Database.Context;
using IdeaMachine.Common.Database.Extensions;
using IdeaMachine.Common.Database.Repository;
using IdeaMachine.Modules.Reaction.DataTypes;
using IdeaMachine.Modules.Reaction.DataTypes.Entity;
using IdeaMachine.Modules.Reaction.Repository.Context;
using IdeaMachine.Modules.Reaction.Repository.Interface;

namespace IdeaMachine.Modules.Reaction.Repository
{
	public class ReactionRepository : AbstractRepository<ReactionContext>, IReactionRepository
	{
		public ReactionRepository(DbContextFactory<ReactionContext> dbContextFactory)
			: base(dbContextFactory)
		{
		}

		public async Task<bool> PutReaction(Guid userId, int ideaId, LikeState likeState)
		{
			await using var ctx = CreateContext();

			ctx.Reactions.Update(new ReactionEntity()
			{
				IdeaId = ideaId,
				LikeState = likeState,
				UserId = userId,
			});

			return await ctx.SaveChangesAsyncWithResult();
		}
	}
}