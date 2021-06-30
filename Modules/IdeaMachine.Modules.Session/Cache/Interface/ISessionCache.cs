using System;

namespace IdeaMachine.Modules.Session.Cache.Interface
{
	public interface ISessionCache
	{
		void Insert(Abstractions.DataTypes.Interface.ISession session);

		Abstractions.DataTypes.Session? GetSession(Guid key);
	}
}