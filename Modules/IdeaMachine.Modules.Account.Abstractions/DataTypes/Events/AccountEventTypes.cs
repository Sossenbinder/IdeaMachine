using IdeaMachine.Modules.Account.Abstractions.DataTypes.Model;

namespace IdeaMachine.Modules.Account.Abstractions.DataTypes.Events
{
	public record AccountCreated(string UserName, string Email, string VerificationCode);

	public record AccountSignedIn(AccountModel Account);
}