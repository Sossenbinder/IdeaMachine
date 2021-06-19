using ProtoBuf;

namespace IdeaMachine.Modules.Account.DataTypes.Model
{
	[ProtoContract]
	public class RegisterModel
	{
		[ProtoMember(1)]
		public string Email { get; set; } = null!;

		[ProtoMember(2)]
		public string UserName { get; set; } = null!;

		[ProtoMember(3)]
		public string Password { get; set; } = null!;
	}
}