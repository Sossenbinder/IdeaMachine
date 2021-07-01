using System;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Interface;
using ProtoBuf;

namespace IdeaMachine.Modules.Account.Abstractions.DataTypes
{
	[ProtoContract]
	public class AnonymousUser : IUser
	{
		[ProtoMember(1)]
		public Guid UserId { get; set; }

		[ProtoMember(2)]
		public string UserName { get; set; } = "";

		[ProtoMember(3)]
		public string Email { get; set; } = "";

		[ProtoMember(4)]
		public DateTime LastAccessedAt { get; set; } = DateTime.UtcNow;
	}
}