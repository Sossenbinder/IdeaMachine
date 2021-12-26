using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.Modules.Account.DataTypes.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
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

		public SocialLoginController(SignInManager<AccountEntity> signInManager)
		{
			_signInManager = signInManager;
		}

		[HttpGet]
		[Route("GoogleLogin")]
		public IActionResult GoogleLogin()
		{
			var properties = new AuthenticationProperties
			{
				RedirectUri = Url.Action("GoogleOauthCallback")
			};

			return Challenge(properties, GoogleDefaults.AuthenticationScheme);
		}

		[HttpGet]
		[Route("ListAvailableProviders")]
		public async Task<JsonDataResponse<List<string>>> ListAvailableProviders()
		{
			var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();

			return JsonDataResponse<List<string>>.Success(schemes.Select(x => x.DisplayName).ToList());
		}

		[HttpGet]
		[Route("GoogleOauthCallback")]
		public async Task<IActionResult> GoogleOauthCallback()
		{
			var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			var claimsIdentity = result.Principal?.Identities.FirstOrDefault();
			if (claimsIdentity is not null)
			{
				var claims = claimsIdentity.Claims.Select(claim => new
				{
					claim.Issuer,
					claim.OriginalIssuer,
					claim.Type,
					claim.Value
				});
			}

			return Ok();
		}
	}
}