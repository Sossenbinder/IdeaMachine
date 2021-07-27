using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.Pagination;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.DataTypes.UiModels.Idea;
using IdeaMachine.DataTypes.UiModels.Pagination;
using IdeaMachine.Modules.Idea.DataTypes.Model;
using IdeaMachine.Modules.Idea.Service.Interface;
using IdeaMachine.Modules.Reaction.Service.Interface;
using IdeaMachine.Modules.Session.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachine.Controllers
{
	public record GetIdeasUiModel(int? PaginationToken = null);

	[ApiController]
	[Route("[controller]")]
	public class IdeaController : IdentityControllerBase
	{
		private readonly IIdeaService _ideaService;

		private readonly IIdeaRetrievalService _ideaRetrievalService;

		private readonly IReactionRetrievalService _reactionRetrievalService;

		public IdeaController(
			ISessionService sessionService,
			IIdeaService ideaService,
			IIdeaRetrievalService ideaRetrievalService,
			IReactionRetrievalService reactionRetrievalService)
			: base(sessionService)
		{
			_ideaService = ideaService;
			_ideaRetrievalService = ideaRetrievalService;
			_reactionRetrievalService = reactionRetrievalService;
		}

		[HttpPost]
		[Route("Get")]
		public async Task<JsonDataResponse<PaginationResult<int?, IdeaUiModel>>> Get([FromBody] PaginationTokenUiModel<int?> getIdeasTokenModel)
		{
			var result = await _ideaRetrievalService.Get(getIdeasTokenModel.PaginationToken);

			var uiModelPayload = result.WithNewPayload(await Task.WhenAll(result.Data.Select(ToEnrichedUiModel)));
			return JsonResponse.Success(uiModelPayload);
		}

		[HttpGet]
		[Route("GetOwn")]
		public async Task<JsonDataResponse<List<IdeaUiModel>>> GetOwn()
		{
			var result = await _ideaRetrievalService.GetForUser(Session.User.UserId);

			var uiModelPayload = await Task.WhenAll(result.Select(ToEnrichedUiModel));
			return JsonResponse.Success(uiModelPayload.ToList());
		}

		[HttpPost]
		[Route("GetForUser")]
		public async Task<JsonDataResponse<List<IdeaUiModel>>> GetForUser(Guid userId)
		{
			var result = await _ideaRetrievalService.GetForUser(userId);

			var uiModelPayload = await Task.WhenAll(result.Select(ToEnrichedUiModel));
			return JsonResponse.Success(uiModelPayload.ToList());
		}

		[HttpPost]
		[Route("GetSpecificIdea")]
		public async Task<JsonDataResponse<IdeaUiModel?>> GetSpecificIdea([FromBody] int id)
		{
			var result = await _ideaRetrievalService.GetSpecificIdea(id);

			if (result.IsFailure)
			{
				return JsonDataResponse<IdeaUiModel?>.Error();
			}

			return JsonDataResponse<IdeaUiModel?>.Success(await ToEnrichedUiModel(result.PayloadOrFail!));
		}

		[HttpPost]
		[Route("Add")]
		public async Task<JsonResponse> Add([FromBody] IdeaModel ideaModel)
		{
			await _ideaService.Add(Session, ideaModel);

			return JsonResponse.Success();
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