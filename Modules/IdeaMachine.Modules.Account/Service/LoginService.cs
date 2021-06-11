﻿using System.Threading.Tasks;
using IdeaMachine.Common.AspNetIdentity.DataTypes;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Events;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Model;
using IdeaMachine.Modules.Account.Abstractions.Events.Interface;
using IdeaMachine.Modules.Account.DataTypes.Entity;
using IdeaMachine.Modules.Account.DataTypes.Model;
using IdeaMachine.Modules.Account.Service.Interface;
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

			if (account.EmailConfirmed)
			{
				return ServiceResponse.Success(LoginResult.WithCode(IdentityErrorCode.EmailNotConfirmed));
			}

			var result = await _signInManager.PasswordSignInAsync(account, loginModel.Password, loginModel.RememberMe, false);

			if (result.Succeeded)
			{
				var accountModel = new AccountModel
				{
					Id = account.Id,
					Email = account.Email,
					UserName = account.UserName,
					LastAccessedAt = account.LastAccessedAt,
				};

				await _accountEvents.AccountSignedIn.Raise(new AccountSignedIn(accountModel));

				return ServiceResponse<LoginResult>.Success(new LoginResult()
				{
					ResultCode = IdentityErrorCode.Success,
					Account = accountModel
				});
			}

			return ServiceResponse.Failure(LoginResult.WithCode(IdentityErrorCode.DefaultError));
		}

		private async Task<AccountEntity?> GetAccountOrNull(string emailOrUserName)
		{
			var account = await _userManager.FindByEmailAsync(emailOrUserName) ?? await _userManager.FindByNameAsync(emailOrUserName);

			return account;
		}
	}
}