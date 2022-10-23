using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Common.Core.Utils.Pagination;
using IdeaMachine.Common.Database.Context;
using IdeaMachine.Common.Database.Extensions;
using IdeaMachine.Common.Database.Repository;
using IdeaMachine.Modules.Idea.DataTypes.Entity;
using IdeaMachine.Modules.Idea.DataTypes.Model;
using IdeaMachine.Modules.Idea.Repository.Context;
using IdeaMachine.Modules.Idea.Repository.Interface;
using MassTransit.SignalR.Contracts;
using Microsoft.EntityFrameworkCore;

namespace IdeaMachine.Modules.Idea.Repository
{
	public class IdeaRepository : AbstractRepository<IdeaContext>, IIdeaRepository
	{
		public IdeaRepository(DbContextFactory<IdeaContext> dbContextFactory)
			: base(dbContextFactory)
		{
		}

		public async Task<IdeaEntity> Add(IdeaEntity idea)
		{
			await using var ctx = CreateContext();

			ctx.Ideas.Add(idea);

			if (idea.Tags.IsNotNullOrEmpty())
			{
				ctx.Tags.AddRange(idea.Tags!);
			}

			await ctx.SaveChangesAsync();

			return idea;
		}

		public async Task<PaginationResult<int?, IdeaEntity>> Get(int? paginationToken = null)
		{
			await using var ctx = CreateContext();

			var query = ctx.Ideas
				.AsQueryable();

			if (paginationToken is not null)
			{
				query = query
					.Where(x => x.Id < paginationToken);
			}

			query = query
				.OrderByDescending(x => x.Id)
				.Take(50)
				.Include(x => x.Tags)
				.Include(x => x.AttachmentUrls);

			var queryResult = await query.ToListAsync();

			int? nextContinuationToken = queryResult.Any() ? queryResult.Last().Id : null;

			return new PaginationResult<int?, IdeaEntity>(nextContinuationToken, queryResult);
		}

		public async Task<int> Count(Guid? userId)
		{
			await using var ctx = CreateContext();

			var query = ctx.Ideas
				.AsQueryable();

			if (userId is not null)
			{
				query = query.Where(x => x.CreatorId == userId);
			}

			return await query.CountAsync();
		}

		public async Task<List<IdeaEntity>> GetForSpecificUser(Guid userId)
		{
			await using var ctx = CreateContext();

			return await ctx.Ideas
				.Where(x => x.CreatorId == userId)
				.Include(x => x.Tags)
				.Include(x => x.AttachmentUrls)
				.ToListAsync();
		}

		public async Task<Guid?> GetOwner(int id)
		{
			await using var ctx = CreateContext();

			return await ctx
				.Ideas
				.Where(x => x.Id == id)
				.Select(x => x.CreatorId)
				.FirstOrDefaultAsync();
		}

		public async Task<IdeaEntity?> GetSpecificIdea(int id)
		{
			await using var ctx = CreateContext();

			return await ctx.Ideas
				.Include(x => x.Tags)
				.Include(x => x.AttachmentUrls)
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task MigrateIdeas(Guid oldOwner, Guid newOwner)
		{
			await using var ctx = CreateContext();

			var oldIdeas = await ctx.Ideas
				.Where(x => x.CreatorId == oldOwner)
				.ToListAsync();

			foreach (var ideaOfOldOwner in oldIdeas)
			{
				ideaOfOldOwner.CreatorId = newOwner;
			}

			await ctx.SaveChangesAsync();
		}

		public async Task<IdeaDeleteErrorCode> Delete(Guid userId, int id)
		{
			await using var ctx = CreateContext();

			var idea = await ctx.Ideas.FirstOrDefaultAsync(x => x.Id == id);

			if (idea is null)
			{
				return IdeaDeleteErrorCode.NotFound;
			}

			if (idea.CreatorId != userId)
			{
				return IdeaDeleteErrorCode.NotOwned;
			}

			ctx.Ideas.Remove(idea);

			return await ctx.SaveChangesAsyncWithResult()
				? IdeaDeleteErrorCode.Successful
				: IdeaDeleteErrorCode.UnspecifiedError;
		}

		public async Task<List<AttachmentEntity>> AddAttachmentUrls(int ideaId, IEnumerable<string> attachmentUrls)
		{
			await using var ctx = CreateContext();

			var idea = await ctx.Ideas.FirstOrDefaultAsync(x => x.Id == ideaId);

			if (idea is null)
			{
				return new();
			}

			var attachmentEntities = attachmentUrls.Select(x => new AttachmentEntity()
			{
				AttachmentUrl = x,
			}).ToList();

			if (idea.AttachmentUrls?.Any() ?? false)
			{
				idea.AttachmentUrls.AddRange(attachmentEntities);
			}
			else
			{
				idea.AttachmentUrls = attachmentEntities;
			}

			await ctx.SaveChangesAsync();

			return attachmentEntities;
		}

		public async Task<AttachmentEntity?> GetAttachmentUrl(int ideaId, int id)
		{
			await using var ctx = CreateContext();

			return await ctx
				.AttachmentUrls
				.Include(x => x.Idea)
				.FirstOrDefaultAsync(x => x.Id == id && x.IdeaId == ideaId);
		}

		public async Task DeleteAttachmentUrl(AttachmentEntity entity)
		{
			await using var ctx = CreateContext();

			ctx.AttachmentUrls.Remove(entity);

			await ctx.SaveChangesAsync();
		}

		public async Task AddComment(CommentEntity entity)
		{
			await using var ctx = CreateContext();

			ctx.Comments.Add(entity);

			await ctx.SaveChangesAsync();
		}
	}
}