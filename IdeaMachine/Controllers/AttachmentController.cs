using System.Threading.Tasks;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.DataTypes.UiModels.Idea;
using IdeaMachine.Extensions;
using IdeaMachine.Modules.Idea.Service.Interface;
using IdeaMachine.Modules.Session.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachine.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AttachmentController : IdentityControllerBase
    {
        private readonly IIdeaAttachmentService _ideaAttachmentService;

        public AttachmentController(
            ISessionService sessionService,
            IIdeaAttachmentService ideaAttachmentService)
            : base(sessionService)
        {
            _ideaAttachmentService = ideaAttachmentService;
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<JsonResponse> Delete([FromBody] DeleteAttachmentUrlUiModel deleteAttachmentUrl)
        {
            var response = await _ideaAttachmentService.RemoveAttachment(Session, deleteAttachmentUrl.IdeaId, deleteAttachmentUrl.AttachmentId);

            return response.ToJsonResponse();
        }
    }
}