using Autofac;
using IdeaMachine.Modules.Session.Service;
using IdeaMachine.Modules.Session.Service.Interface;

namespace IdeaMachine.Modules.Session.DI
{
	public class SessionModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<SessionService>()
				.As<ISessionService>()
				.SingleInstance();
		}
	}
}