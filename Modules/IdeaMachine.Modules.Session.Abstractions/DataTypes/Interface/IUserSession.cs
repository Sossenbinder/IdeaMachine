using System;
using ProtoBuf;

namespace IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface
{
	[ProtoContract]
	[ProtoInclude(1, typeof(AnonymousUserSession))]
	[ProtoInclude(2, typeof(AccountSession))]
	public interface IUserSession
	{
		[ProtoMember(1)]
		bool IsAnonymous { get; }

		[ProtoMember(2)]
		Guid UserId { get; }

		[ProtoMember(3)]
		string? Email { get; }
	}
}