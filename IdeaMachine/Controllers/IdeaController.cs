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
using IdeaMachine.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        private readonly IIdeaAttachmentService _ideaAttachmentService;

        public IdeaController(
            ISessionService sessionService,
            IIdeaService ideaService,
            IIdeaRetrievalService ideaRetrievalService,
            IReactionRetrievalService reactionRetrievalService,
            IIdeaAttachmentService ideaAttachmentService)
            : base(sessionService)
        {
            _ideaService = ideaService;
            _ideaRetrievalService = ideaRetrievalService;
            _reactionRetrievalService = reactionRetrievalService;
            _ideaAttachmentService = ideaAttachmentService;
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

        [AllowAnonymous]
        [HttpPost]
        [Route("Blub")]
        public async Task Blub((int First, string Second) model)
        {
	        await Task.Yield();
        }

        [HttpPost]
        [Route("Add")]
        public async Task<JsonResponse> Add(
            [ModelBinder(BinderType = typeof(MultiPartJsonModelBinder))] IdeaModel ideaModel,
            IFormCollection form)
        {
            if (!ideaModel.Validate())
            {
                return JsonResponse.Error();
            }

            var ideaId = await _ideaService.Add(Session, ideaModel);

            if (form.Files.Any())
            {
                await _ideaAttachmentService.UploadAttachments(Session, form.Files, ideaId);
            }

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