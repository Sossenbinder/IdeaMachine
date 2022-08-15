using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Database.Context;
using IdeaMachine.Common.Database.Repository;
using IdeaMachine.Modules.Idea.DataTypes.Entity;
using IdeaMachine.Modules.Idea.Repository.Context;
using IdeaMachine.Modules.Idea.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace IdeaMachine.Modules.Idea.Repository
{
	public class CommentRepository : AbstractCrudRepository<IdeaContext, CommentEntity>, ICommentRepository
	{
		public CommentRepository(DbContextFactory<IdeaContext> dbContextFactory)
			: base(dbContextFactory)
		{
		}

		public async Task<List<CommentEntity>> GetComments(int ideaId)
		{
			await using var ctx = CreateContext();

			var entities = await ctx.Comments
				.Where(x => x.IdeaId == ideaId)
				.ToListAsync();

			return entities;
		}
	}
}