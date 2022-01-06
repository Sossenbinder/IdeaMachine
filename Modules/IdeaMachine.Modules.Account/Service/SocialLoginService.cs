using System.Security.Claims;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Account.DataTypes.Entity;
using IdeaMachine.Modules.Account.DataTypes.Model;
using IdeaMachine.Modules.Account.Service.Interface;
using Microsoft.AspNetCore.Identity;

namespace IdeaMachine.Modules.Account.Service
{
	public class SocialLoginService : ISocialLoginService
	{
		private readonly UserManager<AccountEntity> _userManager;

		public SocialLoginService(UserManager<AccountEntity> userManager)
		{
			_userManager = userManager;
		}

		public async Task<ServiceResponse> AddExternalUser(SocialLoginInformation socialLoginInformation)
		{
			var loginInfo = socialLoginInformation.ExternalLoginInfo;

			var email = loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
			var username = loginInfo.Principal.FindFirstValue(ClaimTypes.Name);

			AccountEntity? account = default;

			if (email is not null)
			{
				account = await _userManager.FindByEmailAsync(email);
			}

			if (account is null && username is not null)
			{
				account = await _userManager.FindByNameAsync(username);
			}

			if (account is not null)
			{
				return ServiceResponse.Success();
			}

			account = new AccountEntity()
			{
				Email = email,
				UserName = username,
			};

			if (!(await _userManager.CreateAsync(account)).Succeeded)
			{
				return ServiceResponse.Failure();
			}

			if (!(await _userManager.AddLoginAsync(account, loginInfo)).Succeeded)
			{
				return ServiceResponse.Failure();
			}

			return ServiceResponse.Success();
		}
	}
}