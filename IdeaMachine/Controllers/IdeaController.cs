using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.Extensions;
using IdeaMachine.Modules.Idea.DataTypes;
using IdeaMachine.Modules.Idea.Service.Interface;
using IdeaMachine.Modules.Session.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachine.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class IdeaController : IdentityControllerBase
	{
		private readonly IIdeaService _ideaService;

		private readonly IIdeaRetrievalService _ideaRetrievalService;

		public IdeaController(
			ISessionService sessionService,
			IIdeaService ideaService,
			IIdeaRetrievalService ideaRetrievalService)
			: base(sessionService)
		{
			_ideaService = ideaService;
			_ideaRetrievalService = ideaRetrievalService;
		}

		[HttpGet]
		[Route("Get")]
		public async Task<JsonDataResponse<List<IdeaModel>>> Get()
		{
			var result = await _ideaRetrievalService.Get();

			return JsonResponse.Success(result);
		}

		[HttpGet]
		[Route("GetOwn")]
		public async Task<JsonDataResponse<List<IdeaModel>>> GetOwn()
		{
			var result = await _ideaRetrievalService.GetForUser(Session.User.UserId);

			return JsonResponse.Success(result);
		}

		[HttpPost]
		[Route("GetForUser")]
		public async Task<JsonDataResponse<List<IdeaModel>>> GetForUser(Guid userId)
		{
			var result = await _ideaRetrievalService.GetForUser(userId);

			return JsonResponse.Success(result);
		}

		[HttpPost]
		[Route("GetSpecificIdea")]
		public async Task<JsonDataResponse<IdeaModel?>> GetSpecificIdea([FromBody] int id)
		{
			var result = await _ideaRetrievalService.GetSpecificIdea(id);

			return result.ToJsonDataResponse();
		}

		[HttpPost]
		[Route("Add")]
		public async Task Add([FromBody] IdeaModel ideaModel)
		{
			await _ideaService.Add(Session, ideaModel);
		}
	}
}