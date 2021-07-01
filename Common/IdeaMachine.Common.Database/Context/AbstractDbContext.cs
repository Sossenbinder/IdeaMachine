using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace IdeaMachine.Common.Database.Context
{
	public abstract class AbstractDbContext : DbContext
	{
		protected AbstractDbContext([NotNull] string connectionString)
			: base(GetContextOptions(connectionString))
		{
		}

		private static DbContextOptions GetContextOptions([NotNull] string connectionString)
		{
			return new DbContextOptionsBuilder()
				.UseSqlServer(connectionString)
				.Options;
		}
	}
}