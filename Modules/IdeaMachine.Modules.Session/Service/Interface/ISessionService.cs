using IdeaMachine.Modules.Account.Abstractions.DataTypes.Model;

namespace IdeaMachine.Modules.Session.Service.Interface
{
	public interface ISessionService
	{
		AccountModel? GetSession(int userId);
	}
}