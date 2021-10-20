using System.Security.Claims;
using System.Threading.Tasks;
using IdeaMachine.Common.AspNetIdentity.DataTypes;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.DataTypes.UiModels.Account;
using IdeaMachine.Extensions;
using IdeaMachine.Modules.Account.Abstractions.DataTypes;
using IdeaMachine.Modules.Account.DataTypes.Model;
using IdeaMachine.Modules.Account.Service.Interface;
using IdeaMachine.Modules.Session.Service.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachine.Controllers
{
	[Route("Account")]
	[AutoValidateAntiforgeryToken]
	public class AccountController : IdentityControllerBase
	{
		private readonly ILoginService _loginService;

		private readonly IRegistrationService _registrationService;

		public AccountController(
			ISessionService sessionService,
			ILoginService loginService,
			IRegistrationService registrationService)
			: base(sessionService)
		{
			_loginService = loginService;
			_registrationService = registrationService;
		}

		[Route("Get")]
		[HttpGet]
		public JsonDataResponse<UserUiModel> Get()
		{
			return JsonDataResponse<UserUiModel>.Success(new UserUiModel()
			{
				UserId = Session.User.UserId,
				Email = Session.User.Email,
				UserName = Session.User.UserName,
				IsAnonymous = Session.User is AnonymousUser,
				LastAccessedAt = Session.User.LastAccessedAt,
			});
		}

		[Route("Logout")]
		[Authorize]
		[HttpPost]
		public async Task<JsonResponse> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return JsonResponse.Success();
		}

		[Route("Register")]
		[HttpPost]
		public async Task<JsonDataResponse<IdentityErrorCode>> Register([FromBody] RegisterUiModel model)
		{
			if (!ModelState.IsValid)
			{
				return JsonDataResponse<IdentityErrorCode>.Error();
			}

			var registrationResponse = await _registrationService.RegisterAccount(new RegisterModel()
			{
				Email = model.Email,
				Password = model.Password,
				UserName = model.UserName,
			});

			return registrationResponse.ToJsonDataResponse();
		}

		[Route("SignIn")]
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<JsonDataResponse<IdentityErrorCode>> SignIn([FromBody] SignInInfo signInInfo)
		{
			if (!ModelState.IsValid)
			{
				return JsonResponse.Error(IdentityErrorCode.DefaultError);
			}

			var loginResponse = await _loginService.Login(new LoginModel()
			{
				EmailUserName = signInInfo.EmailUserName,
				Password = signInInfo.Password,
				RememberMe = signInInfo.RememberMe,
			});

			if (!loginResponse.IsSuccess)
			{
				return JsonResponse.Error(loginResponse.PayloadOrFail.ResultCode);
			}

			var account = loginResponse.PayloadOrFail.Account!;

			var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
			identity.AddClaim(new Claim(ClaimTypes.Name, account.UserId.ToString()));

			var principal = new ClaimsPrincipal(identity);

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				principal,
				new AuthenticationProperties()
				{
					IsPersistent = signInInfo.RememberMe,
				}
			);

			return JsonDataResponse<IdentityErrorCode>.Success();
		}
	}
}