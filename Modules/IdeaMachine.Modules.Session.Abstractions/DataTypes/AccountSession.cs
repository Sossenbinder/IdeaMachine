using System;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;
using ProtoBuf;

namespace IdeaMachine.Modules.Session.Abstractions.DataTypes
{
	[ProtoContract]
	public class AccountSession : IUserSession
	{
		public bool IsAnonymous { get; } = false;

		[ProtoMember(1)]
		public Guid UserId { get; set; }

		[ProtoMember(2)]
		public string UserName { get; set; } = null!;

		[ProtoMember(3)]
		public string Email { get; set; } = null!;

		[ProtoMember(4)]
		public DateTime LastAccessedAt { get; set; }
	}
}