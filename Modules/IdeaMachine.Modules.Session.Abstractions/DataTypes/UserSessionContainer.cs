using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;
using ProtoBuf;

namespace IdeaMachine.Modules.Session.Abstractions.DataTypes
{
	[ProtoContract]
	public abstract class UserSessionContainer : IUserSessionContainer
	{
		[ProtoMember(50)]
		public ISession Session { get; set; } = null!;

		protected UserSessionContainer()
		{
		}

		protected UserSessionContainer(ISession session)
		{
			Session = session;
		}
	}
}