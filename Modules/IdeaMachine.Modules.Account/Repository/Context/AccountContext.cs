using IdeaMachine.Modules.Account.DataTypes.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdeaMachine.Modules.Account.Repository.Context
{
	public class AccountContext : IdentityDbContext<AccountEntity, IdentityRole<int>, int>
	{
		// Migration
		//public AccountContext()
		//	: base(new DbContextOptionsBuilder<AccountContext>().UseNpgsql("Server=ideamachine.postgres;Port=5432;User Id=ideamachine;Password=ideamachine;Database=ideamachine;").Options)
		//{
		//}

		public AccountContext(DbContextOptions<AccountContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<AccountEntity>()
				.Property(p => p.Id)
				.ValueGeneratedOnAdd();
		}
	}
}