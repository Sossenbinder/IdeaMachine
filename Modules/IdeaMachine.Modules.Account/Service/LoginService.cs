using System;
using System.Threading.Tasks;
using IdeaMachine.Common.AspNetIdentity.DataTypes;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Events;
using IdeaMachine.Modules.Account.Abstractions.Events.Interface;
using IdeaMachine.Modules.Account.DataTypes.Entity;
using IdeaMachine.Modules.Account.DataTypes.Model;
using IdeaMachine.Modules.Account.Service.Interface;
using IdeaMachine.Modules.Session.Abstractions.DataTypes;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;
using Microsoft.AspNetCore.Identity;

namespace IdeaMachine.Modules.Account.Service
{
	public class LoginService : ILoginService
	{
		private readonly UserManager<AccountEntity> _userManager;

		private readonly IAccountEvents _accountEvents;

		private readonly SignInManager<AccountEntity> _signInManager;

		public LoginService(
			UserManager<AccountEntity> userManager,
			IAccountEvents accountEvents,
			SignInManager<AccountEntity> signInManager)
		{
			_userManager = userManager;
			_accountEvents = accountEvents;
			_signInManager = signInManager;
		}

		public async Task<ServiceResponse<LoginResult>> Login(LoginModel loginModel)
		{
			var account = await GetAccountOrNull(loginModel.EmailUserName);

			if (account is null)
			{
				return ServiceResponse.Failure(LoginResult.WithCode(IdentityErrorCode.InvalidEmailOrUserName));
			}

			if (!account.EmailConfirmed)
			{
				return ServiceResponse.Failure(LoginResult.WithCode(IdentityErrorCode.EmailNotConfirmed));
			}
			
			if (!await _userManager.CheckPasswordAsync(account, loginModel.Password))
			{
				return ServiceResponse<LoginResult>.Failure(LoginResult.WithCode(IdentityErrorCode.PasswordMismatch));
			}

			var result = await _signInManager.PasswordSignInAsync(account, loginModel.Password, loginModel.RememberMe, false);

			if (!result.Succeeded)
			{
				return ServiceResponse.Failure(LoginResult.WithCode(IdentityErrorCode.DefaultError));
			}

			var accountModel = account.ToModel();

			await _accountEvents.AccountSignedIn.Raise(new AccountSignedIn(accountModel));

			return ServiceResponse<LoginResult>.Success(new LoginResult()
			{
				ResultCode = IdentityErrorCode.Success,
				Account = accountModel
			});

		}

		public async Task Logout(ISession session)
		{
			await _accountEvents.AccountLoggedOut.Raise(new AccountLoggedOut(session.User));
		}

		public async Task RefreshLogin(RefreshLoginModel refreshLoginModel)
		{
			var account = await _userManager.FindByIdAsync(refreshLoginModel.UserId.ToString());

			if (account is not null)
			{
				var accountModel = new Abstractions.DataTypes.Account()
				{
					UserId = account.Id,
					Email = account.Email,
					UserName = account.UserName,
					LastAccessedAt = account.LastAccessedAt,
				};

				await _accountEvents.AccountSignedIn.Raise(new AccountSignedIn(accountModel));
			}
		}

		private async Task<AccountEntity?> GetAccountOrNull(string emailOrUserName)
		{
			var account = await _userManager.FindByEmailAsync(emailOrUserName) ?? await _userManager.FindByNameAsync(emailOrUserName);

			return account;
		}
	}
}