using System.Threading.Tasks;
using IdeaMachine.Common.AspNetIdentity.DataTypes;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.DataTypes.UiModels;
using IdeaMachine.Extensions;
using IdeaMachine.Modules.Account.DataTypes.Model;
using IdeaMachine.Modules.Account.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachine.Controllers
{
	[Route("Account")]
	[AutoValidateAntiforgeryToken]
	public class AccountController : Controller
	{
		private readonly ILoginService _loginService;

		private readonly IRegistrationService _registrationService;

		public AccountController(
			ILoginService loginService,
			IRegistrationService registrationService)
		{
			_loginService = loginService;
			_registrationService = registrationService;
		}

		[Route("Register")]
		[HttpPost]
		public async Task<JsonDataResponse<IdentityErrorCode?>> Register([FromBody] RegisterUiModel model)
		{
			var registrationResponse = await _registrationService.RegisterAccount(new RegisterModel()
			{
				Email = model.Email,
				Password = model.Password,
				UserName = model.UserName,
			});

			return registrationResponse.ToJsonResponse();
		}

		[Route("SignIn")]
		[HttpPost]
		public async Task<IActionResult> SignIn([FromBody] SignInInfo signInInfo)
		{
			await Task.CompletedTask;

			return Ok();
		}
	}
}