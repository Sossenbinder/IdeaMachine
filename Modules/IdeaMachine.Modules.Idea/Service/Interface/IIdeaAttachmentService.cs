using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Idea.DataTypes.Model;
using Microsoft.AspNetCore.Http;
using ISession = IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface.ISession;

namespace IdeaMachine.Modules.Idea.Service.Interface
{
    public interface IIdeaAttachmentService
    {
	    Task<List<AttachmentModel>> UploadAttachments(ISession session, IFormFileCollection files, int ideaId);

        Task<List<string>> GetAttachments(ISession session, int ideaId);

        Task<ServiceResponse> RemoveAttachment(ISession session, int ideaId, int attachmentId);
    }
}