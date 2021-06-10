using Autofac;
using IdeaMachine.Modules.Account.Events;
using IdeaMachine.Modules.Account.Events.Interface;
using IdeaMachine.Modules.Account.Service;
using IdeaMachine.Modules.Account.Service.Interface;

namespace IdeaMachine.Modules.Account.DI
{
	public class AccountModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<RegistrationService>()
				.As<IRegistrationService>()
				.SingleInstance();

			builder.RegisterType<LoginService>()
				.As<ILoginService>()
				.SingleInstance();

			builder.RegisterType<AccountEvents>()
				.As<IAccountEvents>()
				.SingleInstance();
		}
	}
}