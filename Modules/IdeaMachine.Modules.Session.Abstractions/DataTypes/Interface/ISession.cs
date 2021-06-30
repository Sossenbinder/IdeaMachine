using IdeaMachine.Modules.Account.Abstractions.DataTypes.Interface;
using ProtoBuf;

namespace IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface
{
	[ProtoContract]
	[ProtoInclude(100, typeof(Session))]
	public interface ISession
	{
		bool IsAnonymous { get; }

		[ProtoMember(1)]
		IUser User { get; set; }
	}
}