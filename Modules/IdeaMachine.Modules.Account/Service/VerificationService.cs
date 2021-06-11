using System.Threading.Tasks;
using IdeaMachine.Common.AspNetIdentity.DataTypes;
using IdeaMachine.Common.AspNetIdentity.Extension;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Account.DataTypes.Entity;
using IdeaMachine.Modules.Account.Service.Interface;
using Microsoft.AspNetCore.Identity;

namespace IdeaMachine.Modules.Account.Service
{
	public class VerificationService : IVerificationService
	{
		private readonly UserManager<AccountEntity> _userManager;

		public VerificationService(UserManager<AccountEntity> userManager)
		{
			_userManager = userManager;
		}

		public async Task<ServiceResponse<IdentityErrorCode>> VerifyAccount(string userName, string token)
		{
			var user = await _userManager.FindByNameAsync(userName);

			if (user is null)
			{
				return ServiceResponse.Failure(IdentityErrorCode.InvalidUserName);
			}

			var result = await _userManager.ConfirmEmailAsync(user, token);

			return result.Succeeded
				? ServiceResponse.Success(IdentityErrorCode.Success)
				: ServiceResponse.Failure(result.FirstErrorOrFail());
		}
	}
}