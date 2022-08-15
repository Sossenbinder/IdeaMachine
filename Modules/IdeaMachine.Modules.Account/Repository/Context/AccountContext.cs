using IdeaMachine.Common.Database.Context;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Entity;
using Microsoft.EntityFrameworkCore;

namespace IdeaMachine.Modules.Account.Repository.Context
{
	public class AccountContext : AbstractDbContext
	{
		public DbSet<UserInfoEntity> UserInfo { get; set; }

		public AccountContext(string connectionString)
			: base(connectionString)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<UserInfoEntity>()
				.ToTable("UserInfo")
				.HasKey(x => x.UserId);
		}
	}
}