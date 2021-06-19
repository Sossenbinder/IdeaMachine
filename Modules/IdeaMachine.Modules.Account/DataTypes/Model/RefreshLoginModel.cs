using System;
using ProtoBuf;

namespace IdeaMachine.Modules.Account.DataTypes.Model
{
	[ProtoContract]
	public class RefreshLoginModel
	{
		[ProtoMember(1)]
		public Guid UserId { get; set; }
	}
}