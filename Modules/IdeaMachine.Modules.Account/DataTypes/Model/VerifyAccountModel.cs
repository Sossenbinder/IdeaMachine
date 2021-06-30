using IdeaMachine.Modules.Session.Abstractions.DataTypes;
using ProtoBuf;

namespace IdeaMachine.Modules.Account.DataTypes.Model
{
	[ProtoContract]
	public class VerifyAccountModel : UserSessionContainer
	{
		[ProtoMember(1)]
		public string UserName { get; set; }

		[ProtoMember(2)]
		public string Token { get; set; }
	}
}