using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.Modules.Idea.Service.Interface;
using IdeaMachineWeb.DataTypes.UiModels.Idea;
using IdeaMachineWeb.Extensions;
using IdeaMachineWeb.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachineWeb.Controllers
{
	public record UploadIdeaModel(int IdeaId);

	[ApiController]
	[Route("[controller]")]
	public class AttachmentController : IdentityControllerBase
	{
		private readonly IIdeaAttachmentService _ideaAttachmentService;

		public AttachmentController(IIdeaAttachmentService ideaAttachmentService)
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