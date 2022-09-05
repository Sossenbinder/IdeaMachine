using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Cache;
using IdeaMachine.Common.Core.Cache.Implementations.Interface;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Interface;
using IdeaMachine.Modules.ServiceBase;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;
using IdeaMachine.Modules.Session.Service.Interface;

namespace IdeaMachine.Modules.Session.Service
{
	public class SessionService : ServiceBaseWithoutLogger, ISessionService
	{
		private readonly IDistributedCache<Guid, ISession> _sessionCache;

		public SessionService(RedisCacheFactory redisCacheFactory)
		{
			_sessionCache = redisCacheFactory.Create<Guid, ISession>();
		}

		public async ValueTask AddSession(Guid userId, ISession session)
		{
			await _sessionCache.Set(userId, session);
		}

		public Task<ISession?> GetSession(Guid userId)
		{
			return _sessionCache.GetOrDefault(userId);
		}

		public Task<bool> HasSession(Guid userId)
		{
			return _sessionCache.Has(userId);
		}

		public async Task UpdateSession(Guid userId, Action<ISession> sessionUpdater)
		{
			await using var lockedItem = await _sessionCache.TryGetLocked(userId);

			if (lockedItem is not null)
			{
				sessionUpdater(lockedItem.Value);
			}
		}

		private static Guid GetKey(IUser user) => user.UserId;
	}
}