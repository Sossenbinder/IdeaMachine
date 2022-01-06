using Microsoft.AspNetCore.Identity;
using ProtoBuf;

namespace IdeaMachine.Modules.Account.DataTypes.Model
{
	[ProtoContract]
	public class SocialLoginInformation
	{
		[ProtoMember(1)]
		public ExternalLoginInfo ExternalLoginInfo { get; set; }
	}
}