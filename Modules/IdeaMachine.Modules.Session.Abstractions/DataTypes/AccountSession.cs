using System;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;
using ProtoBuf;

namespace IdeaMachine.Modules.Session.Abstractions.DataTypes
{
	[ProtoContract]
	public class AccountSession : IUserSession
	{
		[ProtoMember(1)]
		public bool IsAnonymous { get; } = false;

		[ProtoMember(2)]
		public Guid UserId { get; set; }

		[ProtoMember(3)]
		public string UserName { get; set; } = null!;

		[ProtoMember(4)]
		public string Email { get; set; } = null!;

		[ProtoMember(5)]
		public DateTime LastAccessedAt { get; set; }
	}
}