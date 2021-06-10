using System;
using IdeaMachine.Modules.Session.DataTypes.Interface;

namespace IdeaMachine.Modules.Session.DataTypes
{
	public class AnonymousUserSession : IUserSession
	{
		public bool IsAnonymous { get; } = true;

		public Guid UserId { get; }

		public string? Email { get; } = null;

		public AnonymousUserSession(Guid userId) => UserId = userId;
	}
}