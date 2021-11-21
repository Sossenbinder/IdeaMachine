using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Model;
using IdeaMachine.Modules.Account.DataTypes.Entity;
using IdeaMachine.Modules.Account.Service.Interface;
using Microsoft.AspNetCore.Identity;

namespace IdeaMachine.Modules.Account.Service
{
    public class AccountService : IAccountService
	{
		private readonly UserManager<AccountEntity> _userManager;

		public AccountService(UserManager<AccountEntity> userManager)
		{
			_userManager = userManager;
		}

		public async Task<ServiceResponse<string>> GetAccountName(GetAccountNameRequest getAccountNameModel)
		{
			var user = await _userManager.FindByIdAsync(getAccountNameModel.UserIdentifier.ToString());

			return user is not null
				? ServiceResponse<string>.Success(user.UserName)
				: ServiceResponse<string>.Failure();
		}
    }
}
