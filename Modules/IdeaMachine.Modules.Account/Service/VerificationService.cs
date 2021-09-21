using System.Threading.Tasks;
using IdeaMachine.Common.AspNetIdentity.DataTypes;
using IdeaMachine.Common.AspNetIdentity.Extension;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Events;
using IdeaMachine.Modules.Account.Abstractions.Events.Interface;
using IdeaMachine.Modules.Account.DataTypes.Entity;
using IdeaMachine.Modules.Account.DataTypes.Model;
using IdeaMachine.Modules.Account.Service.Interface;
using Microsoft.AspNetCore.Identity;

namespace IdeaMachine.Modules.Account.Service
{
	public class VerificationService : IVerificationService
	{
		private readonly IAccountEvents _accountEvents;

		private readonly UserManager<AccountEntity> _userManager;

		public VerificationService(
			IAccountEvents accountEvents,
			UserManager<AccountEntity> userManager)
		{
			_accountEvents = accountEvents;
			_userManager = userManager;
		}

		public async Task<ServiceResponse<IdentityErrorCode>> VerifyAccount(VerifyAccountModel verifyModel)
		{
			var user = await _userManager.FindByNameAsync(verifyModel.UserName);

			if (user is null)
			{
				return ServiceResponse.Failure(IdentityErrorCode.InvalidEmailOrUserName);
			}

			var result = await _userManager.ConfirmEmailAsync(user, verifyModel.Token);

			if (!result.Succeeded)
			{
				return ServiceResponse.Failure(result.FirstErrorOrFail());
			}

			await _accountEvents.AccountVerified.Raise(new AccountVerified(verifyModel.Session.User, user.ToModel()));

			return ServiceResponse.Success(IdentityErrorCode.Success);
		}
	}
}