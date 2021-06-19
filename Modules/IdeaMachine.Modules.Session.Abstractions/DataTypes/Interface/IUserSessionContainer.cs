using ProtoBuf;

namespace IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface
{
	[ProtoContract]
	[ProtoInclude(100, typeof(UserSessionContainer))]
	public interface IUserSessionContainer
	{
		IUserSession Session { get; set; }
	}
}