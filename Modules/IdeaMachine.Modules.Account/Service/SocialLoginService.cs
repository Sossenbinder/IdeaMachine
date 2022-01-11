using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Extensions;
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
			var (email, userName) = socialLoginInformation;

			new List<int>().ToList()
			AccountEntity? account = default;

			if (email.IsNotNullOrEmpty())
			{
				account = await _userManager.FindByEmailAsync(email);
			}

			if (userName.IsNotNullOrEmpty())
			{
				account = await _userManager.FindByNameAsync(userName);
			}

			if (account is not null)
			{
				return ServiceResponse.Success();
			}

			account = new AccountEntity()
			{
				Email = email,
				UserName = userName,
			};

			if (!(await _userManager.CreateAsync(account)).Succeeded)
			{
				return ServiceResponse.Failure();
			}

			//if (!(await _userManager.AddLoginAsync(account, loginInfo)).Succeeded)
			//{
			//	return ServiceResponse.Failure();
			//}

			return ServiceResponse.Success();
		}
	}
}