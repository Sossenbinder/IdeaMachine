using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.Modules.Account.DataTypes.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IdeaMachine.AccountService.Controller
{
	[ApiController]
	[Route("[controller]")]
	public class SocialLoginController : ControllerBase
	{
		private readonly ILogger<SocialLoginController> _logger;

		private readonly SignInManager<AccountEntity> _signInManager;

		private readonly UserManager<AccountEntity> _userManager;

		public SocialLoginController(
			ILogger<SocialLoginController> logger,
			SignInManager<AccountEntity> signInManager,
			UserManager<AccountEntity> userManager)
		{
			_logger = logger;
			_signInManager = signInManager;
			_userManager = userManager;
		}

		[HttpGet]
		[Route("ListAvailableProviders")]
		public async Task<JsonDataResponse<List<string>>> ListAvailableProviders()
		{
			var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();

			return JsonDataResponse<List<string>>.Success(schemes.Select(x => x.DisplayName).ToList()!);
		}
	}
}