using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.Modules.Account.DataTypes.Entity;
using IdeaMachineWeb.DataTypes.UiModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Ocsp;

namespace IdeaMachineWeb.Controllers
{
	[ApiController]
	[AllowAnonymous]
	[Route("SocialLogin")]
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

		[HttpGet]
		[Route("ExternalLogin")]
		public IActionResult ExternalLogin([FromQuery] string provider, [FromQuery] bool rememberMe)
		{
			var forwardedHeader = Request.Headers["X-Forwarded-For"].FirstOrDefault();
			var redirectUrl = Url.Action("SocialLoginCallback", "SocialLogin", new
				{
					rememberMe
				}, HttpContext.Request.Scheme, host: forwardedHeader ?? Request.Host.Value);

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
				_logger.LogError("Someone tried to login, but no information was returned from the external login");
				return SocialLoginRedirect(SocialLoginResponseCode.InfoUnavailable);
			}

			var signinResult = await _signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, rememberMe, true);

			if (signinResult.Succeeded)
			{
				_logger.LogInformation("");
				return RedirectToAction("Index", "Home");
			}

			// No associated login, let's provide an account
			var email = loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
			var userName = loginInfo.Principal.FindFirstValue(ClaimTypes.Name);

			if (email is null)
			{
				if (userName is not null)
				{
					_logger.LogError("External login attempted for unknown user, but no email was found. Username: {0}", userName);
				}

				return SocialLoginRedirect(SocialLoginResponseCode.EmailNotKnown);
			}

			var account = await _userManager.FindByEmailAsync(email);

			var creationResult = await _userManager.CreateAsync(new AccountEntity()
			{
				Email = email,
				UserName = userName,
			});

			if (account is null && !creationResult.Succeeded)
			{
				_logger.LogError("User with email {0} and name {1} tried to login, but no account was found and generation failed. Errors: {3}", email, userName, creationResult
					.Errors
					.Select(x => $"\n{x.Code} - {x.Description}"));
				return SocialLoginRedirect(SocialLoginResponseCode.CouldntCreateAccount);
			}

			await _userManager.AddLoginAsync(account!, loginInfo);
			await _signInManager.SignInAsync(account!, false);

			return RedirectToAction("Index", "Home");
		}

		private IActionResult SocialLoginRedirect(SocialLoginResponseCode code) => Redirect($"/Logon/Error/{(int)code}");
	}
}