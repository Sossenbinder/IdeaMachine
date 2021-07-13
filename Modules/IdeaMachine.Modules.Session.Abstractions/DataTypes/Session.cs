using IdeaMachine.Modules.Account.Abstractions.DataTypes;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Interface;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;
using ProtoBuf;

namespace IdeaMachine.Modules.Session.Abstractions.DataTypes
{
	[ProtoContract]
	public class Session : ISession
	{
		public bool IsAnonymous => User is AnonymousUser;

		[ProtoMember(1)]
		public IUser User { get; set; } = null!;
	}
}