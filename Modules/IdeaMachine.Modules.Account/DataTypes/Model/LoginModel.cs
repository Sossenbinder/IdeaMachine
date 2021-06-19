using ProtoBuf;

namespace IdeaMachine.Modules.Account.DataTypes.Model
{
	[ProtoContract]
	public class LoginModel
	{
		[ProtoMember(1)]
		public string EmailUserName { get; set; } = null!;

		[ProtoMember(2)]
		public string Password { get; set; } = null!;

		[ProtoMember(3)]
		public bool RememberMe { get; set; }
	}
}