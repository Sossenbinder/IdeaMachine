using ProtoBuf;

namespace IdeaMachine.Modules.Account.DataTypes.Model
{
	[ProtoContract]
	public record SocialLoginInformation(
		[property: ProtoMember(1)] string Email,
		[property: ProtoMember(2)] string Name);
}