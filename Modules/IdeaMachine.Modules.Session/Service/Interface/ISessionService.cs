using System;
using System.Threading.Tasks;

namespace IdeaMachine.Modules.Session.Service.Interface
{
	public interface ISessionService
	{
		Abstractions.DataTypes.Session? GetSession(Guid userId);

		Task UpdateSession(Guid userId, Action<Abstractions.DataTypes.Session> sessionUpdater);
	}
}