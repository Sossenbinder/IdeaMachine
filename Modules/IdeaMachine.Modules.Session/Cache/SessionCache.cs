using System;
using IdeaMachine.Common.Core.Utils.Caching;
using IdeaMachine.Modules.Session.Abstractions.DataTypes;
using IdeaMachine.Modules.Session.Cache.Interface;

namespace IdeaMachine.Modules.Session.Cache
{
	public class SessionCache : ISessionCache
	{
		private readonly TypedMemoryCache<Guid, AccountSession> _sessions;

		public SessionCache()
		{
			_sessions = new TypedMemoryCache<Guid, AccountSession>();
		}

		public void Insert(AccountSession account)
		{
			_sessions.Set(account.UserId, account);
		}

		public AccountSession? GetSession(Guid key)
		{
			return _sessions.TryGetValue(key, out var accountModel) ? accountModel : null;
		}
	}
}