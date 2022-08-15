using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Web.Extensions;
using IdeaMachine.Modules.Idea.Service.Interface;
using IdeaMachineWeb.DataTypes.UiModels.Idea;
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
		public async Task<IActionResult> Upload(
			[ModelBinder(BinderType = typeof(MultiPartJsonModelBinder))] UploadIdeaModel uploadIdeaModel,
			IFormCollection form)
		{
			if (!form.Files.Any())
			{
				return BadRequest();
			}

			var results = await _ideaAttachmentService.UploadAttachments(Session, form.Files, uploadIdeaModel.IdeaId);

			return Json(results.First().Id);
		}

		[HttpDelete]
		[Route("Delete")]
		public async Task<IActionResult> Delete([FromBody] DeleteAttachmentUrlUiModel deleteAttachmentUrl)
		{
			var response = await _ideaAttachmentService.RemoveAttachment(Session, deleteAttachmentUrl.IdeaId, deleteAttachmentUrl.AttachmentId);

			return response.AsHttpResponse();
		}
	}
}