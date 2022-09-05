using Autofac;
using IdeaMachine.Modules.Account.Service;
using IdeaMachine.Modules.Account.Service.Interface;

namespace IdeaMachine.Modules.Account.DI
{
	public class AccountModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<LastAccessedService>()
				.As<ILastAccessedService>()
				.SingleInstance();
		}
	}
}