using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;

namespace IdeaMachine.Modules.Idea.Service.Interface
{
    public interface IIdeaAttachmentService
    {
        Task UploadAttachments(ISession session, Microsoft.AspNetCore.Http.IFormFileCollection files, int ideaId);

        Task<List<string>> GetAttachments(ISession session, int ideaId);

        Task<ServiceResponse> RemoveAttachment(ISession session, int ideaId, int attachmentId);
    }
}