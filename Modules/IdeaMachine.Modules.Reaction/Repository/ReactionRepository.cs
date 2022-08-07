using System;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Common.Database.Context;
using IdeaMachine.Common.Database.Extensions;
using IdeaMachine.Common.Database.Repository;
using IdeaMachine.Modules.Reaction.Abstractions.DataTypes;
using IdeaMachine.Modules.Reaction.DataTypes.Entity;
using IdeaMachine.Modules.Reaction.Repository.Context;
using IdeaMachine.Modules.Reaction.Repository.Interface;
using Microsoft.EntityFrameworkCore;

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

			await ctx.Reactions.AddOrUpdate(new ReactionEntity()
			{
				IdeaId = ideaId,
				LikeState = likeState,
				UserId = userId,
			}, x => x.IdeaId == ideaId && x.UserId == userId);

			return await ctx.SaveChangesAsyncWithResult();
		}

		public async Task<int> GetTotalLikeCount(int ideaId)
		{
			await using var ctx = CreateContext();

			var ratedEntities = await ctx.Reactions.Where(x => x.IdeaId == ideaId && x.LikeState != LikeState.Neutral).ToListAsync();

			if (ratedEntities.IsNullOrEmpty())
			{
				return 0;
			}

			var result = ratedEntities
				.Sum(x => x.LikeState switch
				{
					LikeState.Dislike => -1,
					LikeState.Like => 1,
					_ => 0,
				});

			return result;
		}

		public async Task<LikeState> GetLikeState(int ideaId, Guid userId)
		{
			await using var ctx = CreateContext();

			var match = await ctx.Reactions.FirstOrDefaultAsync(x => x.IdeaId == ideaId && x.UserId == userId);

			return match?.LikeState ?? LikeState.Neutral;
		}
	}
}