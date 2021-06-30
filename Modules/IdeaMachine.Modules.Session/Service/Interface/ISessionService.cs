using System;
using IdeaMachine.Modules.Session.Abstractions.DataTypes;

namespace IdeaMachine.Modules.Session.Service.Interface
{
	public interface ISessionService
	{
		Abstractions.DataTypes.Session? GetSession(Guid userId);
	}
}