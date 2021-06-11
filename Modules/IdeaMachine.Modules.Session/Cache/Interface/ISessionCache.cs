using IdeaMachine.Modules.Account.Abstractions.DataTypes.Model;

namespace IdeaMachine.Modules.Session.Cache.Interface
{
	public interface ISessionCache
	{
		void Insert(AccountModel account);

		AccountModel? GetSession(int key);
	}
}