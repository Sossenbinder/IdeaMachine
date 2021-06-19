using System;
using ProtoBuf;

namespace IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface
{
	[ProtoContract]
	[ProtoInclude(100, typeof(AnonymousUserSession))]
	[ProtoInclude(101, typeof(AccountSession))]
	public interface IUserSession
	{
		bool IsAnonymous { get; }

		[ProtoMember(1)]
		Guid UserId { get; set; }

		[ProtoMember(2)]
		string? Email { get; set; }
	}
}