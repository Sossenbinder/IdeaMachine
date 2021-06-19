using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;
using ProtoBuf;

namespace IdeaMachine.Modules.Session.Abstractions.DataTypes
{
	[ProtoContract]
	public abstract class UserSessionContainer : IUserSessionContainer
	{
		public IUserSession Session { get; set; } = null!;

		protected UserSessionContainer()
		{
		}

		protected UserSessionContainer(IUserSession session)
		{
			Session = session;
		}
	}
}