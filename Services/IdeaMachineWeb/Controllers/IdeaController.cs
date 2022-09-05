using System;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Modules.Idea.DataTypes.Model;
using IdeaMachine.Modules.Idea.Service.Interface;
using IdeaMachine.Modules.Reaction.Service.Interface;
using IdeaMachine.Service.Base.Controller;
using IdeaMachineWeb.DataTypes.UiModels.Idea;
using IdeaMachineWeb.DataTypes.UiModels.Pagination;
using IdeaMachineWeb.DataTypes.Validation;
using IdeaMachineWeb.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IdeaMachineWeb.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class IdeaController : IdentityControllerBase
	{
		private readonly IIdeaService _ideaService;

		private readonly IIdeaRetrievalService _ideaRetrievalService;

		private readonly IReactionRetrievalService _reactionRetrievalService;

		private readonly IIdeaAttachmentService _ideaAttachmentService;

		private readonly IdeaValidationInfo _validationInfo;

		public IdeaController(
			IIdeaService ideaService,
			IIdeaRetrievalService ideaRetrievalService,
			IReactionRetrievalService reactionRetrievalService,
			IIdeaAttachmentService ideaAttachmentService,
			IOptions<ValidationInfo> validationInfo)
		{
			_ideaService = ideaService;
			_ideaRetrievalService = ideaRetrievalService;
			_reactionRetrievalService = reactionRetrievalService;
			_ideaAttachmentService = ideaAttachmentService;
			_validationInfo = validationInfo.Value.Idea;
		}

		[HttpPost]
		public async Task<IActionResult> Get([FromBody] PaginationTokenUiModel<int?> getIdeasTokenModel)
		{
			var result = await _ideaRetrievalService.Get(getIdeasTokenModel.PaginationToken);

			var uiModelPayload = result.WithNewPayload(await Task.WhenAll(result.Data.Select(ToEnrichedUiModel)));
			return Json(uiModelPayload);
		}

		[HttpGet]
		[Route("GetOwn")]
		public async Task<IActionResult> GetOwn()
		{
			var result = await _ideaRetrievalService.GetForUser(Session);

			var uiModelPayload = await Task.WhenAll(result.Select(ToEnrichedUiModel));
			return Json(uiModelPayload.ToList());
		}

		[HttpPost]
		[Route("GetForUser")]
		public async Task<IActionResult> GetForUser(Guid userId)
		{
			var result = await _ideaRetrievalService.GetForUser(Session);

			var uiModelPayload = await Task.WhenAll(result.Select(ToEnrichedUiModel));
			return Json(uiModelPayload.ToList());
		}

		[HttpPost]
		[Route("GetSpecificIdea")]
		public async Task<IActionResult> GetSpecificIdea([FromBody] int id)
		{
			var result = await _ideaRetrievalService.GetSpecificIdea(Session, id);

			return result.IsFailure
				? InternalServerError()
				: Json(await ToEnrichedUiModel(result.PayloadOrFail!));
		}

		[HttpPost]
		[Route("Add")]
		public async Task<IActionResult> Add(
			[ModelBinder(BinderType = typeof(MultiPartJsonModelBinder))] IdeaModel ideaModel,
			IFormCollection form)
		{
			if (!ideaModel.Validate())
			{
				return BadRequest(AddIdeaResult.ValidationFailure);
			}

			var hasFormFiles = form.Files.Any();

			if (hasFormFiles)
			{
				if (form.Files.Count > _validationInfo.MaxUploads)
				{
					return BadRequest(AddIdeaResult.TooManyUploads);
				}

				if (form.Files.Any(x => x.Length == _validationInfo.MaxByteSize))
				{
					return BadRequest(AddIdeaResult.UploadTooBig);
				}
			}

			var ideaId = await _ideaService.Add(Session, ideaModel);

			if (hasFormFiles)
			{
				await _ideaAttachmentService.UploadAttachments(Session, form.Files, ideaId);
			}

			return Ok();
		}

		[HttpDelete]
		[Route("Delete")]
		public async Task<IActionResult> Delete([FromBody] int id)
		{
			var result = await _ideaService.Delete(Session, id);

			return Json(result);
		}

		private async Task<IdeaUiModel> ToEnrichedUiModel(IdeaModel ideaModel)
		{
			var ideaUiModel = IdeaUiModel.FromModel(ideaModel);

			ideaUiModel.IdeaReactionMetaData = await _reactionRetrievalService.GetIdeaReactionMetaData(ideaUiModel.Id, UserId);

			return ideaUiModel;
		}
	}
}