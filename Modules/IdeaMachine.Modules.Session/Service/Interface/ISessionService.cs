using System;
using System.Threading.Tasks;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;

namespace IdeaMachine.Modules.Session.Service.Interface
{
	public interface ISessionService
	{
		ValueTask AddSession(Guid userId, ISession session);

		Task<ISession?> GetSession(Guid userId);

		Task<bool> HasSession(Guid userId);

		Task UpdateSession(Guid userId, Action<ISession> sessionUpdater);
	}
}