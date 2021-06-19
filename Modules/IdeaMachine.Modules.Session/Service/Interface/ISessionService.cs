using System;
using IdeaMachine.Modules.Session.Abstractions.DataTypes;

namespace IdeaMachine.Modules.Session.Service.Interface
{
	public interface ISessionService
	{
		AccountSession? GetSession(Guid userId);
	}
}