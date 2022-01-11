using Autofac;
using IdeaMachine.Modules.Account.Service;
using IdeaMachine.Modules.Account.Service.Interface;

namespace IdeaMachine.Modules.Account.DI
{
	public class InternalAccountModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterModule<AccountModule>();

			builder.RegisterType<RegistrationService>()
				.As<IRegistrationService>()
				.SingleInstance();

			builder.RegisterType<LoginService>()
				.As<ILoginService>()
				.SingleInstance();

			builder.RegisterType<AccountService>()
				.As<IAccountService>()
				.SingleInstance();

			builder.RegisterType<VerificationService>()
				.As<IVerificationService>()
				.SingleInstance();

			base.Load(builder);
		}
	}
}