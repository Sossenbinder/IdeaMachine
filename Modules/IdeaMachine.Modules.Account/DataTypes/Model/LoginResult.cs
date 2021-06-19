using IdeaMachine.Common.AspNetIdentity.DataTypes;
using IdeaMachine.Modules.Session.Abstractions.DataTypes;
using ProtoBuf;

namespace IdeaMachine.Modules.Account.DataTypes.Model
{
	[ProtoContract]
	public class LoginResult
	{
		[ProtoMember(1)]
		public IdentityErrorCode ResultCode { get; set; }

		[ProtoMember(2)]
		public AccountSession? Account { get; set; }

		public static LoginResult WithCode(IdentityErrorCode errorCode) => new() { ResultCode = errorCode };
	}
}