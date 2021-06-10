using IdeaMachine.Common.Database.Context;
using IdeaMachine.Modules.Idea.DataTypes.Entity;
using Microsoft.EntityFrameworkCore;

namespace IdeaMachine.Modules.Idea.Repository.Context
{
	public class IdeaContext : AbstractDbContext
	{
		public DbSet<IdeaEntity> Ideas { get; set; } = null!;

		public IdeaContext(string connectionString)
			: base(connectionString)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<IdeaEntity>()
				.Property(x => x.Id)
				.ValueGeneratedOnAdd();
		}
	}
}