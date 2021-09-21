using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IdeaMachine.Common.Database.Context;
using IdeaMachine.Common.Database.Extensions;
using IdeaMachine.Common.Database.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace IdeaMachine.Common.Database.Repository
{
    public abstract class AbstractCrudRepository<TDbContext, TEntity> : AbstractRepository<TDbContext>, ICrudRepository<TEntity>
        where TDbContext : AbstractDbContext
        where TEntity : class
    {
		protected AbstractCrudRepository(DbContextFactory<TDbContext> dbContextFactory) 
			: base(dbContextFactory)
		{
		}

		public async Task<bool> Add(TEntity entity)
		{
			await using var ctx = CreateContext();

			GetDbSet(ctx).Add(entity);

			return await ctx.SaveChangesAsyncWithResult();
		}

	    public async Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
		{
			await using var ctx = CreateContext();

			return await GetDbSet(ctx)
				.Where(predicate)
				.ToListAsync();
		}

	    private static DbSet<TEntity> GetDbSet(TDbContext ctx) => ctx.Set<TEntity>();
	}
}
