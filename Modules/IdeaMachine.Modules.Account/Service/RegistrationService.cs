using System.Threading.Tasks;
using IdeaMachine.Common.AspNetIdentity.DataTypes;
using IdeaMachine.Common.AspNetIdentity.Extension;
using IdeaMachine.Common.Core.Utils.IPC;
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

		public RegistrationService(
			UserManager<AccountEntity> userManager,
			IPasswordHasher<AccountEntity> passwordHasher)
		{
			_userManager = userManager;
			_passwordHasher = passwordHasher;
		}

		public async Task<ServiceResponse<IdentityErrorCode?>> RegisterAccount(RegisterModel registerModel)
		{
			var accountEntity = new AccountEntity()
			{
				Email = registerModel.Email,
				UserName = registerModel.UserName,
			};

			accountEntity.PasswordHash = _passwordHasher.HashPassword(accountEntity, registerModel.Password);

			var createResult = await _userManager.CreateAsync(accountEntity, registerModel.Password);

			return createResult.Succeeded
				? ServiceResponse.Success(IdentityErrorCode.Success as IdentityErrorCode?)
				: ServiceResponse.Failure(createResult.FirstErrorOrNull());
		}
	}
}