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
	public class RegistrationService : IRegistrationService
	{
		private readonly UserManager<AccountEntity> _userManager;

		private readonly IPasswordHasher<AccountEntity> _passwordHasher;

		private readonly IAccountEvents _accountEvents;

		public RegistrationService(
			UserManager<AccountEntity> userManager,
			IPasswordHasher<AccountEntity> passwordHasher,
			IAccountEvents accountEvents)
		{
			_userManager = userManager;
			_passwordHasher = passwordHasher;
			_accountEvents = accountEvents;
		}

		public async Task<ServiceResponse<IdentityErrorCode>> RegisterAccount(RegisterModel registerModel)
		{
			var accountEntity = new AccountEntity()
			{
				Email = registerModel.Email,
				UserName = registerModel.UserName,
			};

			accountEntity.PasswordHash = _passwordHasher.HashPassword(accountEntity, registerModel.Password);

			var createResult = await _userManager.CreateAsync(accountEntity, registerModel.Password);

			if (!createResult.Succeeded)
			{
				return ServiceResponse.Failure(createResult.FirstErrorOrFail());
			}

			var emailVerificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(accountEntity);

			await _accountEvents.AccountCreated.Raise(new AccountCreated(registerModel.UserName, registerModel.Email, emailVerificationToken));

			return ServiceResponse.Success(IdentityErrorCode.Success);
		}
	}
}