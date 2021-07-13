using IdeaMachine.Common.Database.Context;
using IdeaMachine.Modules.Reaction.DataTypes.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace IdeaMachine.Modules.Reaction.Repository.Context
{
	public class ReactionContext : AbstractDbContext
	{
		public DbSet<ReactionEntity> Reactions { get; set; } = null!;

		public ReactionContext(string connectionString)
			: base(connectionString)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ReactionEntity>()
				.HasKey(x => new
				{
					x.IdeaId,
					x.UserId,
				});
		}
	}
}