using System;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;
using ProtoBuf;

namespace IdeaMachine.Modules.Session.Abstractions.DataTypes
{
	[ProtoContract]
	public class AnonymousUserSession : IUserSession
	{
		public bool IsAnonymous { get; set; } = true;

		[ProtoMember(1)]
		public Guid UserId { get; set; }

		[ProtoMember(2)]
		public string? Email { get; set; } = null;

		public AnonymousUserSession(Guid userId) => UserId = userId;
	}
}