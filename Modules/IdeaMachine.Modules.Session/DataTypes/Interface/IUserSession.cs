using System;

namespace IdeaMachine.Modules.Session.DataTypes.Interface
{
	public interface IUserSession
	{
		bool IsAnonymous { get; }

		Guid UserId { get; }

		string? Email { get; }
	}
}