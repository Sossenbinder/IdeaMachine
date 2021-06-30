using System;
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
	public class IdeaRepository : AbstractRepository<IdeaContext>, IIdeaRepository
	{
		public IdeaRepository(DbContextFactory<IdeaContext> dbContextFactory)
			: base(dbContextFactory)
		{
		}

		public async Task<IdeaEntity> Add(IdeaEntity idea)
		{
			await using var ctx = CreateContext();

			ctx.Ideas.Add(idea);

			await ctx.SaveChangesAsync();

			return idea;
		}

		public async Task<List<IdeaEntity>> Get()
		{
			await using var ctx = CreateContext();

			return await ctx.Ideas.ToListAsync();
		}

		public async Task<List<IdeaEntity>> GetForSpecificUser(Guid userId)
		{
			await using var ctx = CreateContext();

			return await ctx.Ideas.Where(x => x.Creator == userId).ToListAsync();
		}

		public async Task<IdeaEntity?> GetSpecificIdea(int id)
		{
			await using var ctx = CreateContext();

			return await ctx.Ideas.FirstOrDefaultAsync(x => x.Id == id);
		}
	}
}