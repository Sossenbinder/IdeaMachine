using IdeaMachine.Common.Database.Context;
using IdeaMachine.Modules.Idea.DataTypes.Entity;
using Microsoft.EntityFrameworkCore;

namespace IdeaMachine.Modules.Idea.Repository.Context
{
	public class IdeaContext : AbstractDbContext
	{
		public DbSet<IdeaEntity> Ideas { get; set; } = null!;

		public DbSet<TagEntity> Tags { get; set; } = null!;

		public DbSet<AttachmentEntity> AttachmentUrls { get; set; } = null!;

		public DbSet<CommentEntity> Comments { get; set; } = null!;

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
				.Property(x => x.CreatorId)
				.HasColumnName("Creator");

			modelBuilder.Entity<IdeaEntity>()
				.HasMany(x => x.Tags)
				.WithOne(x => x.Idea);

			modelBuilder.Entity<IdeaEntity>()
				.HasMany(x => x.AttachmentUrls)
				.WithOne(x => x.Idea);

			modelBuilder.Entity<IdeaEntity>()
				.HasMany(x => x.Comments)
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

			modelBuilder.Entity<AttachmentEntity>()
				.Property(x => x.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<AttachmentEntity>()
				.HasOne(x => x.Idea)
				.WithMany(x => x.AttachmentUrls)
				.HasForeignKey(x => x.IdeaId);

			modelBuilder.Entity<AttachmentEntity>()
				.ToTable("AttachmentUrls")
				.HasKey(x => x.Id);

			modelBuilder.Entity<CommentEntity>()
				.Property(x => x.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<CommentEntity>()
				.HasOne(x => x.Idea)
				.WithMany(x => x.Comments)
				.HasForeignKey(x => x.IdeaId);

			modelBuilder.Entity<CommentEntity>()
				.ToTable("Comments")
				.HasKey(x => x.Id);
		}
	}
}