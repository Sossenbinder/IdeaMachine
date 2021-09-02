using IdeaMachine.Common.Database.Context;
using IdeaMachine.Modules.Idea.DataTypes.Entity;
using Microsoft.EntityFrameworkCore;

namespace IdeaMachine.Modules.Idea.Repository.Context
{
	public class IdeaContext : AbstractDbContext
	{
		public DbSet<IdeaEntity> Ideas { get; set; } = null!;

		public DbSet<TagEntity> Tags { get; set; } = null!;

		public IdeaContext(string connectionString)
			: base(connectionString)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<IdeaEntity>()
				.Property(x => x.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<IdeaEntity>()
				.HasKey(x => x.Id);

			modelBuilder.Entity<IdeaEntity>()
				.HasMany(x => x.Tags)
				.WithOne(x => x.Idea);

			modelBuilder.Entity<TagEntity>()
				.Property(x => x.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<TagEntity>()
				.HasOne(x => x.Idea)
				.WithMany(x => x.Tags)
				.HasForeignKey(x => x.IdeaId);

			modelBuilder.Entity<TagEntity>()
				.HasKey(x => x.Id);
		}
	}
}