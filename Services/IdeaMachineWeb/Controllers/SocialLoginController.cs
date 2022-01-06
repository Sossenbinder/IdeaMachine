using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.Modules.Account.DataTypes.Entity;
using IdeaMachine.Modules.Account.DataTypes.Model;
using IdeaMachine.Modules.Account.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachineWeb.Controllers
{
	[ApiController]
	[AllowAnonymous]
	[Route("SocialLogin")]
	public class SocialLoginController : ControllerBase
	{
		private readonly SignInManager<AccountEntity> _signInManager;

		private readonly ISocialLoginService _socialLoginService;

		public SocialLoginController(
			SignInManager<AccountEntity> signInManager,
			ISocialLoginService socialLoginService)
		{
			_signInManager = signInManager;
			_socialLoginService = socialLoginService;
		}

		[HttpGet]
		[Route("ListAvailableProviders")]
		public async Task<JsonDataResponse<List<string>>> ListAvailableProviders()
		{
			var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();

			return JsonDataResponse<List<string>>.Success(schemes.Select(x => x.DisplayName).ToList()!);
		}

		[HttpGet]
		[Route("ExternalLogin")]
		public IActionResult ExternalLogin([FromQuery] string provider)
		{
			var redirectUrl = Url.Action(nameof(SocialLoginCallback), "SocialLogin");
			var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
			return Challenge(properties, provider);
		}

		[HttpGet]
		[Route("SocialLoginCallback")]
		public async Task<IActionResult> SocialLoginCallback()
		{
			var info = await _signInManager.GetExternalLoginInfoAsync();

			if (info is null)
			{
				return Redirect("/Logon/login");
			}

			var signinResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);

			if (signinResult.Succeeded)
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				var result = await _socialLoginService.AddExternalUser(new SocialLoginInformation { ExternalLoginInfo = info });
			}

			return Redirect("/Logon/login/associate");
			return Ok();
		}
	}
}