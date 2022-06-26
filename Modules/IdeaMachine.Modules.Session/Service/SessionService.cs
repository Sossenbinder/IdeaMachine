using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Cache.Implementations;
using IdeaMachine.Common.Core.Cache.Implementations.Interface;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Events;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Interface;
using IdeaMachine.Modules.Account.Abstractions.Events.Interface;
using IdeaMachine.Modules.ServiceBase;
using IdeaMachine.Modules.Session.Service.Interface;

namespace IdeaMachine.Modules.Session.Service
{
	public class SessionService : ServiceBaseWithoutLogger, ISessionService
	{
		private readonly ICache<Guid, Abstractions.DataTypes.Session> _sessionCache;

		public SessionService(IAccountEvents accountEvents)
		{
			_sessionCache = new MemoryCache<Guid, Abstractions.DataTypes.Session>();

			RegisterEventHandler(accountEvents.AccountSignedIn, OnAccountSignedIn);
			RegisterEventHandler(accountEvents.AccountSignedOut, OnAccountSignedOut);
		}

		private async Task OnAccountSignedOut(AccountLoggedOut account)
		{
			await _sessionCache.Delete(GetKey(account.Session));
		}

		private async Task OnAccountSignedIn(AccountSignedIn accountSignedIn)
		{
			var session = new Abstractions.DataTypes.Session()
			{
				User = accountSignedIn.Account,
			};

			await _sessionCache.Set(session.User.UserId, session);
		}

		public Abstractions.DataTypes.Session? GetSession(Guid userId)
		{
			return _sessionCache.TryGetValue(userId, out var value) ? value : null;
		}

		public async Task UpdateSession(Guid userId, Action<Abstractions.DataTypes.Session> sessionUpdater)
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