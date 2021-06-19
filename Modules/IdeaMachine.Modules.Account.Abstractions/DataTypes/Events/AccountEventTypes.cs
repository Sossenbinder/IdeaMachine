using IdeaMachine.Modules.Session.Abstractions.DataTypes;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;

namespace IdeaMachine.Modules.Account.Abstractions.DataTypes.Events
{
	public record AccountCreated(string UserName, string Email, string VerificationCode);

	public record AccountSignedIn(AccountSession Account);

	public record AccountLoggedOut(IUserSession UserSession);
}