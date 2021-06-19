using System;
using IdeaMachine.Modules.Session.Abstractions.DataTypes;

namespace IdeaMachine.Modules.Session.Cache.Interface
{
	public interface ISessionCache
	{
		void Insert(AccountSession account);

		AccountSession? GetSession(Guid key);
	}
}