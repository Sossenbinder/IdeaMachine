using IdeaMachine.Common.AspNetIdentity.DataTypes;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Model;

namespace IdeaMachine.Modules.Account.DataTypes.Model
{
	public class LoginResult
	{
		public IdentityErrorCode ResultCode { get; set; }

		public AccountModel? Account { get; set; }

		public static LoginResult WithCode(IdentityErrorCode errorCode) => new() { ResultCode = errorCode };
	}
}