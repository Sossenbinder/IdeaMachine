using System;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;
using IdeaMachine.Modules.Session.Cache.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace IdeaMachine.Modules.Session.Cache
{
	public class SessionCache : ISessionCache
	{
		private readonly IMemoryCache _sessions;

		public SessionCache(IMemoryCache sessionsCache)
		{
			_sessions = sessionsCache;
		}

		public void Insert(ISession session)
		{
			_sessions.Set(session.User.UserId, session);
		}

		public Abstractions.DataTypes.Session? GetSession(Guid key)
		{
			return _sessions.TryGetValue<Abstractions.DataTypes.Session?>(key, out var accountModel) ? accountModel : null;
		}
	}
}