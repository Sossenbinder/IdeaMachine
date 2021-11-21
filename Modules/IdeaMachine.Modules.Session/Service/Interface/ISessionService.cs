using System;

namespace IdeaMachine.Modules.Session.Service.Interface
{
	public interface ISessionService
	{
		Abstractions.DataTypes.Session? GetSession(Guid userId);

		void UpdateSession(Guid userId, Action<Abstractions.DataTypes.Session?> sessionUpdater);
	}
}