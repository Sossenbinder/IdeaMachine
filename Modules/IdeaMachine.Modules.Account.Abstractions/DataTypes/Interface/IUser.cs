using System;
using ProtoBuf;

namespace IdeaMachine.Modules.Account.Abstractions.DataTypes.Interface
{
	[ProtoContract]
	public interface IUser
	{
		[ProtoMember(1)]
		public Guid UserId { get; set; }

		[ProtoMember(2)]
		public string UserName { get; set; }

		[ProtoMember(3)]
		public string Email { get; set; }

		[ProtoMember(4)]
		public DateTime LastAccessedAt { get; set; }
	}
}