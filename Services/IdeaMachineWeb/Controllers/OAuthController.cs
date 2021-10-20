using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachineWeb.Controllers
{
    [ApiController]
    [AllowAnonymous]
	[Route("Oauth")]
    public class OAuthController : ControllerBase
    {
	    public OAuthController()
	    {
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
		[Route("GoogleOauthCallback")]
	    public async Task<IActionResult> GoogleOauthCallback()
		{
			var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			var claims = result.Principal.Identities
				.FirstOrDefault().Claims.Select(claim => new
				{
					claim.Issuer,
					claim.OriginalIssuer,
					claim.Type,
					claim.Value
				});

			return Ok();
	    }        
    }
}
