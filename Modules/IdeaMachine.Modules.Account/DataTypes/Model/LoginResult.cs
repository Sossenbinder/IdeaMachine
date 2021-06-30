using IdeaMachine.Common.AspNetIdentity.DataTypes;
using ProtoBuf;

namespace IdeaMachine.Modules.Account.DataTypes.Model
{
	[ProtoContract]
	public class LoginResult
	{
		[ProtoMember(1)]
		public IdentityErrorCode ResultCode { get; set; }

		[ProtoMember(2)]
		public Abstractions.DataTypes.Account? Account { get; set; }

		public static LoginResult WithCode(IdentityErrorCode errorCode) => new() { ResultCode = errorCode };
	}
}