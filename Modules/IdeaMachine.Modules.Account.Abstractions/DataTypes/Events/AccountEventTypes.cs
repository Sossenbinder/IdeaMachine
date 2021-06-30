using IdeaMachine.Modules.Account.Abstractions.DataTypes.Interface;

namespace IdeaMachine.Modules.Account.Abstractions.DataTypes.Events
{
	public record AccountCreated(string UserName, string Email, string VerificationCode);

	public record AccountSignedIn(IUser Account);

	public record AccountLoggedOut(IUser Session);

	public record AccountVerified(IUser OldAnonymousUser, IUser NewUser);
}