using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.Modules.Account.DataTypes.Entity;
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

		private readonly UserManager<AccountEntity> _userManager;

		public SocialLoginController(
			SignInManager<AccountEntity> signInManager,
			UserManager<AccountEntity> userManager)
		{
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

		[HttpGet]
		[Route("ExternalLogin")]
		public IActionResult ExternalLogin([FromQuery] string provider, [FromQuery] bool rememberMe)
		{
			var redirectUrl = Url.Action("SocialLoginCallback", "SocialLogin", new
			{
				rememberMe
			});
			var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
			return Challenge(properties, provider);
		}

		// TODO: Move this endpoint to the accountservice
		[HttpGet]
		[Route("SocialLoginCallback")]
		public async Task<IActionResult> SocialLoginCallback([FromQuery] bool rememberMe)
		{
			// Grab the external login information from the http context
			var loginInfo = await _signInManager.GetExternalLoginInfoAsync();

			if (loginInfo is null)
			{
				return Redirect("/Logon/login");
			}

			var signinResult = await _signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, rememberMe, true);

			if (signinResult.Succeeded)
			{
				return RedirectToAction("Index", "Home");
			}

			// No associated login, let's provide an account

			var email = loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
			var userName = loginInfo.Principal.FindFirstValue(ClaimTypes.Name);

			if (email is null)
			{
				return Problem();
			}

			var account = await _userManager.FindByEmailAsync(email);

			if (account is null && !(await _userManager.CreateAsync(new AccountEntity()
			{
				Email = email,
				UserName = userName,
			})).Succeeded)
			{
				return Problem();
			}

			await _userManager.AddLoginAsync(account!, loginInfo);
			await _signInManager.SignInAsync(account!, false);

			return Ok();
		}
	}
}