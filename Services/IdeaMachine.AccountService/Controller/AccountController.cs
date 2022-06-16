using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using IdeaMachine.AccountService.DataTypes.UiModels.Account;
using IdeaMachine.Common.AspNetIdentity.DataTypes;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.Common.Web.Extensions;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.UiModels;
using IdeaMachine.Modules.Account.DataTypes.Model;
using IdeaMachine.Modules.Account.Service.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachine.AccountService.Controller
{
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IRegistrationService _registrationService;

		private readonly ILoginService _loginService;

		public AccountController(
			IRegistrationService registrationService,
			ILoginService loginService)
		{
			_registrationService = registrationService;
			_loginService = loginService;
		}

		[Route("Register")]
		[AllowAnonymous]
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
		public async Task<JsonDataResponse<IdentityErrorCode>> SignIn([FromBody] SignInInfo signInInfo)
		{
			if (!ModelState.IsValid)
			{
				return JsonResponse.Error(IdentityErrorCode.DefaultError);
			}

			var loginResponse = await _loginService.Login(new LoginModel
			{
				EmailUserName = signInInfo.EmailUserName,
				Password = signInInfo.Password,
				RememberMe = signInInfo.RememberMe,
			});

			if (!loginResponse.IsSuccess)
			{
				var resultCode = loginResponse.PayloadOrFail.ResultCode;
				var statusCode = HttpStatusCode.InternalServerError;

				if (resultCode is IdentityErrorCode.DuplicateEmail or IdentityErrorCode.LoginAlreadyAssociated)
				{
					statusCode = HttpStatusCode.BadRequest;
				}

				return JsonResponse.Error(loginResponse.PayloadOrFail.ResultCode, statusCode);
			}

			var account = loginResponse.PayloadOrFail.Account!;

			var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
			identity.AddClaim(new Claim(ClaimTypes.Name, account.UserId.ToString()));

			var principal = new ClaimsPrincipal(identity);

			await HttpContext.SignInAsync(
				IdentityConstants.ApplicationScheme,
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