using IdeaMachine.Common.Database.Context;

namespace IdeaMachine.Common.Database.Repository
{
	public abstract class AbstractRepository<TDbContext>
		where TDbContext : AbstractDbContext
	{
		private readonly DbContextFactory<TDbContext> _dbContextFactory;

		protected AbstractRepository(DbContextFactory<TDbContext> dbContextFactory)
		{
			_dbContextFactory = dbContextFactory;
		}

		protected TDbContext CreateContext() => _dbContextFactory.CreateDbContext();
	}
}