using System;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Interface;
using ProtoBuf;

namespace IdeaMachine.Modules.Account.Abstractions.DataTypes
{
	[ProtoContract]
	public class Account : IUser
	{
		[ProtoMember(1)]
		public Guid UserId { get; set; }

		[ProtoMember(2)]
		public string UserName { get; set; } = null!;

		[ProtoMember(3)]
		public string Email { get; set; } = null!;

		[ProtoMember(4)]
		public DateTime LastAccessedAt { get; set; }

		[ProtoMember(5)] 
		public string? ProfilePictureUrl { get; set; }
	}
}