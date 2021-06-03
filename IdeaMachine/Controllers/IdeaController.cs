using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.Modules.Idea.DataTypes;
using IdeaMachine.Modules.Idea.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachine.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class IdeaController : ControllerBase
	{
		private readonly IIdeaService _ideaService;

		public IdeaController(IIdeaService ideaService)
		{
			_ideaService = ideaService;
		}

		[HttpGet]
		[Route("Get")]
		public async Task<JsonDataResponse<List<IdeaModel>>> Get()
		{
			var result = await _ideaService.Get();

			return JsonResponse.Success(result);
		}

		[HttpPost]
		[Route("Add")]
		public async Task Add([FromBody] IdeaModel ideaModel)
		{
			await _ideaService.Add(ideaModel);
		}
	}
}