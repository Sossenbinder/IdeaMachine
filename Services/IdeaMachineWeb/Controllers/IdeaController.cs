using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.Pagination;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.Modules.Idea.DataTypes.Model;
using IdeaMachine.Modules.Idea.Service.Interface;
using IdeaMachine.Modules.Reaction.Service.Interface;
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
		[Route("Get")]
		public async Task<JsonDataResponse<PaginationResult<int?, IdeaUiModel>>> Get([FromBody] PaginationTokenUiModel<int?> getIdeasTokenModel)
		{
			var result = await _ideaRetrievalService.Get(Session, getIdeasTokenModel.PaginationToken);

			var uiModelPayload = result.WithNewPayload(await Task.WhenAll(result.Data.Select(ToEnrichedUiModel)));
			return JsonResponse.Success(uiModelPayload);
		}

		[HttpGet]
		[Route("GetOwn")]
		public async Task<JsonDataResponse<List<IdeaUiModel>>> GetOwn()
		{
			var result = await _ideaRetrievalService.GetForUser(Session);

			var uiModelPayload = await Task.WhenAll(result.Select(ToEnrichedUiModel));
			return JsonResponse.Success(uiModelPayload.ToList());
		}

		[HttpPost]
		[Route("GetForUser")]
		public async Task<JsonDataResponse<List<IdeaUiModel>>> GetForUser(Guid userId)
		{
			var result = await _ideaRetrievalService.GetForUser(Session);

			var uiModelPayload = await Task.WhenAll(result.Select(ToEnrichedUiModel));
			return JsonResponse.Success(uiModelPayload.ToList());
		}

		[HttpPost]
		[Route("GetSpecificIdea")]
		public async Task<JsonDataResponse<IdeaUiModel?>> GetSpecificIdea([FromBody] int id)
		{
			var result = await _ideaRetrievalService.GetSpecificIdea(Session, id);

			return result.IsFailure
				? JsonDataResponse<IdeaUiModel?>.Error()
				: JsonDataResponse<IdeaUiModel?>.Success(await ToEnrichedUiModel(result.PayloadOrFail!));
		}

		[HttpPost]
		[Route("Add")]
		public async Task<JsonDataResponse<AddIdeaResult>> Add(
			[ModelBinder(BinderType = typeof(MultiPartJsonModelBinder))] IdeaModel ideaModel,
			IFormCollection form)
		{
			if (!ideaModel.Validate())
			{
				return JsonResponse.Error(AddIdeaResult.ValidationFailure);
			}

			var hasFormFiles = form.Files.Any();

			if (hasFormFiles)
			{
				if (form.Files.Count > _validationInfo.MaxUploads)
				{
					return JsonResponse.Error(AddIdeaResult.TooManyUploads);
				}

				if (form.Files.Any(x => x.Length == _validationInfo.MaxByteSize))
				{
					return JsonResponse.Error(AddIdeaResult.UploadTooBig);
				}
			}

			var ideaId = await _ideaService.Add(Session, ideaModel);

			if (hasFormFiles)
			{
				await _ideaAttachmentService.UploadAttachments(Session, form.Files, ideaId);
			}

			return JsonDataResponse<AddIdeaResult>.Success();
		}

		[HttpDelete]
		[Route("Delete")]
		public async Task<JsonDataResponse<IdeaDeleteErrorCode>> Delete([FromBody] int id)
		{
			var result = await _ideaService.Delete(Session, id);

			return JsonResponse.Success(result);
		}

		private async Task<IdeaUiModel> ToEnrichedUiModel(IdeaModel ideaModel)
		{
			var ideaUiModel = IdeaUiModel.FromModel(ideaModel);

			ideaUiModel.IdeaReactionMetaData = await _reactionRetrievalService.GetIdeaReactionMetaData(ideaUiModel.Id, UserId);

			return ideaUiModel;
		}
	}
}