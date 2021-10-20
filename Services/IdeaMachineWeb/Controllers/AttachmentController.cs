using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.DataTypes.UiModels.Idea;
using IdeaMachine.Extensions;
using IdeaMachine.Modules.Idea.Service.Interface;
using IdeaMachine.Modules.Session.Service.Interface;
using IdeaMachine.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachine.Controllers
{
	public record UploadIdeaModel(int IdeaId);

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

		[HttpPost]
		[Route("Upload")]
		public async Task<JsonDataResponse<int>> Upload(
			[ModelBinder(BinderType = typeof(MultiPartJsonModelBinder))] UploadIdeaModel uploadIdeaModel,
			IFormCollection form)
		{
			if (form.Files.Any())
			{
				var results = await _ideaAttachmentService.UploadAttachments(Session, form.Files, uploadIdeaModel.IdeaId);

				return JsonResponse.Success(results.First().Id);
			}

			return JsonDataResponse<int>.Error();
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