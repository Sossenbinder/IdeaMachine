using Autofac;
using IdeaMachine.Common.Database.Extensions;
using IdeaMachine.Modules.Account.Repository;
using IdeaMachine.Modules.Account.Repository.Context;
using IdeaMachine.Modules.Account.Repository.Interface;
using IdeaMachine.Modules.Account.Service;
using IdeaMachine.Modules.Account.Service.Interface;

namespace IdeaMachine.Modules.Account.DI
{
	public class InternalAccountModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterModule<AccountModule>();

			builder.RegisterType<AccountService>()
				.As<IAccountService>()
				.SingleInstance();

			builder.RegisterDbContextFactory(cfg => new AccountContext(cfg["DbConnectionString"]));

			builder.RegisterType<UserInfoRepository>()
				.As<IUserInfoRepository>()
				.SingleInstance();

			base.Load(builder);
		}
	}
}