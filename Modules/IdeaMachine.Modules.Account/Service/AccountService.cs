using System.Threading.Tasks;
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

		public async Task<GetAccountName.Response> GetAccountName(GetAccountName.Request getAccountNameModel)
		{
			var user = await _userManager.FindByIdAsync(getAccountNameModel.UserIdentifier.ToString());

			return user is not null ? new GetAccountName.Response(user.UserName) : new GetAccountName.Response(null);
		}
    }
}
