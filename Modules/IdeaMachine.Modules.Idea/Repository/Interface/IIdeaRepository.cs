using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.Pagination;
using IdeaMachine.Modules.Idea.DataTypes.Entity;
using IdeaMachine.Modules.Idea.DataTypes.Model;

namespace IdeaMachine.Modules.Idea.Repository.Interface
{
    public interface IIdeaRepository
    {
        Task<IdeaEntity> Add(IdeaEntity idea);

        Task<PaginationResult<int?, IdeaEntity>> Get(int? paginationToken = null);

        Task<List<IdeaEntity>> GetForSpecificUser(Guid userId);

        Task<IdeaEntity?> GetSpecificIdea(int id);

        Task MigrateIdeas(Guid oldOwner, Guid newOwner);

        Task<IdeaDeleteErrorCode> Delete(Guid userId, int id);

        Task<List<AttachmentUrlEntity>> AddAttachmentUrls(int ideaId, IEnumerable<string> attachmentUrls);

        Task<AttachmentUrlEntity?> GetAttachmentUrl(int ideaId, int id);

        Task DeleteAttachmentUrl(AttachmentUrlEntity entity);
    }
}