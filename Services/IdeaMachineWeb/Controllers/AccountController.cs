using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using IdeaMachine.Common.AspNetIdentity.DataTypes;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.Modules.Account.Abstractions.DataTypes;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Events;
using IdeaMachine.Modules.Account.DataTypes.Model;
using IdeaMachine.Modules.Account.Service.Interface;
using IdeaMachine.Modules.Session.Service.Interface;
using IdeaMachineWeb.DataTypes.UiModels.Account;
using IdeaMachineWeb.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachineWeb.Controllers
{
	[Route("Account")]
	[AutoValidateAntiforgeryToken]
	public class AccountController : IdentityControllerBase
	{
		private readonly ILoginService _loginService;

		private readonly IRegistrationService _registrationService;

		private readonly IPublishEndpoint _publishEndpoint;

		public AccountController(
			ILoginService loginService,
			IRegistrationService registrationService, 
			IPublishEndpoint publishEndpoint)
		{
			_loginService = loginService;
			_registrationService = registrationService;
			_publishEndpoint = publishEndpoint;
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
				ProfilePictureUrl = Session.User.ProfilePictureUrl,
			});
		}

		[Route("Logout")]
		[Authorize]
		[HttpPost]
		public async Task<JsonResponse> Logout()
		{
			await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
			await _publishEndpoint.Publish(new AccountLoggedOut(Session.User));
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