using System.Threading.Tasks;
using IdeaMachine.Common.AspNetIdentity.DataTypes;
using IdeaMachine.Common.Web.DataTypes.Responses;
using IdeaMachine.Common.Web.Extensions;
using IdeaMachine.Modules.Account.DataTypes.Model;
using IdeaMachine.Modules.Account.Service.Interface;
using IdeaMachineWeb.DataTypes.UiModels.Account;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachineWeb.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class VerifyController : IdentityControllerBase
	{
		private readonly IVerificationService _verificationService;

		public VerifyController(IVerificationService verificationService)
		{
			_verificationService = verificationService;
		}

		[Route("VerifyMail")]
		[HttpPost]
		public async Task<JsonDataResponse<IdentityErrorCode>> VerifyMail([FromBody] VerifyEmailUiModel verifyEmailModel)
		{
			if (!ModelState.IsValid)
			{
				return JsonDataResponse<IdentityErrorCode>.Error();
			}

			var (userName, token) = verifyEmailModel;

			var result = await _verificationService.VerifyAccount(new VerifyAccountModel()
			{
				UserName = userName,
				Token = token,
				Session = Session,
			});

			return result.ToJsonDataResponse();
		}
	}
}