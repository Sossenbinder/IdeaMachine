using IdeaMachine.Common.Core.Utils.Caching;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Model;
using IdeaMachine.Modules.Session.Cache.Interface;

namespace IdeaMachine.Modules.Session.Cache
{
	public class SessionCache : ISessionCache
	{
		private readonly TypedMemoryCache<int, AccountModel> _sessions;

		public SessionCache()
		{
			_sessions = new TypedMemoryCache<int, AccountModel>();
		}

		public void Insert(AccountModel account)
		{
			_sessions.Set(account.Id, account);
		}

		public AccountModel? GetSession(int key)
		{
			return _sessions.TryGetValue(key, out var accountModel) ? accountModel : null;
		}
	}
}