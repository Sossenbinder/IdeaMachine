using System.Threading.Tasks;
using IdeaMachine.Modules.Account.DataTypes.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachine.AccountService.Controller
{
	[ApiController]
	public class SocialLoginController : ControllerBase
	{
		private readonly SignInManager<AccountEntity> _signInManager;

		private readonly UserManager<AccountEntity> _userManager;

		public SocialLoginController(
			SignInManager<AccountEntity> signInManager,
			UserManager<AccountEntity> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}

		// TODO: Migrate this endpoint from the regular webservice
		[HttpGet]
		[Route("SocialLoginCallback")]
		public async Task<IActionResult> SocialLoginCallback()
		{
			return Ok();
		}
	}
}