using System;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;
using ProtoBuf;

namespace IdeaMachine.Modules.Session.Abstractions.DataTypes
{
	[ProtoContract]
	public class AnonymousUserSession : IUserSession
	{
		[ProtoMember(1)]
		public bool IsAnonymous { get; } = true;

		[ProtoMember(2)]
		public Guid UserId { get; }

		[ProtoMember(3)]
		public string? Email { get; } = null;

		public AnonymousUserSession(Guid userId) => UserId = userId;
	}
}