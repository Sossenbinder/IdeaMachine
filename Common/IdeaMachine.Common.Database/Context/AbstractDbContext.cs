using Microsoft.EntityFrameworkCore;

namespace IdeaMachine.Common.Database.Context
{
	public abstract class AbstractDbContext : DbContext
	{
		protected AbstractDbContext(string connectionString)
			: base(GetContextOptions(connectionString))
		{
		}

		private static DbContextOptions GetContextOptions(string connectionString)
		{
			return new DbContextOptionsBuilder()
				.UseSqlServer(connectionString)
				.Options;
		}
	}
}