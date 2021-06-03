using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Common.Database.Context;
using IdeaMachine.Common.Database.Repository;
using IdeaMachine.Modules.Idea.DataTypes;
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

		public async Task Add(IdeaModel idea)
		{
			await using var ctx = CreateContext();

			ctx.Ideas.Add(idea);

			await ctx.SaveChangesAsync();
		}

		public async Task<List<IdeaModel>> Get()
		{
			await using var ctx = CreateContext();

			return await ctx.Ideas.ToListAsync();
		}
	}
}