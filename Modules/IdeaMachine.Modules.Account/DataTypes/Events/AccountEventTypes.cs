using IdeaMachine.Modules.Session.DataTypes.Interface;

namespace IdeaMachine.Modules.Account.DataTypes.Events
{
	public record AccountCreated(IUserSession Session);
}