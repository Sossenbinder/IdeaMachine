using System;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Events;
using IdeaMachine.Modules.Account.Abstractions.Events.Interface;
using IdeaMachine.Modules.Session.Cache.Interface;
using IdeaMachine.Modules.Session.Service.Interface;
using IdeaMachine.ModulesServiceBase;

namespace IdeaMachine.Modules.Session.Service
{
	public class SessionService : ServiceBaseWithoutLogger, ISessionService
	{
		private readonly ISessionCache _sessionCache;

		public SessionService(
			IAccountEvents accountEvents,
			ISessionCache sessionCache)
		{
			_sessionCache = sessionCache;

			RegisterEventHandler(accountEvents.AccountSignedIn, OnAccountSignedIn);
		}

		private void OnAccountSignedIn(AccountSignedIn accountSignedIn)
		{
			_sessionCache.Insert(new Abstractions.DataTypes.Session()
			{
				User = accountSignedIn.Account,
			});
		}

		public Abstractions.DataTypes.Session? GetSession(Guid userId)
		{
			return _sessionCache.GetSession(userId);
		}

		public void UpdateSession(Guid userId, Action<Abstractions.DataTypes.Session?> sessionUpdater)
		{
			throw new NotImplementedException();
		}
	}
}