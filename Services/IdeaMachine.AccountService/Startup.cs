using System;
using Autofac;
using IdeaMachine.Common.AspNetIdentity.Helper;
using IdeaMachine.Modules.Account.DataTypes.Entity;
using IdeaMachine.Modules.Account.DI;
using IdeaMachine.Modules.Account.Events;
using IdeaMachine.Modules.Account.Repository.Context;
using IdeaMachine.Modules.Account.Service;
using IdeaMachine.Modules.Account.Service.Interface;
using IdeaMachine.Modules.Session.DI;
using IdeaMachine.Service.Base.Extensions;
using IdeaMachine.Service.Base.Startup;
using MassTransit.AutofacIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdeaMachine.AccountService
{
	public class Startup : CommonEndpointStartup
	{
		public Startup(IConfiguration configuration)
			: base(configuration)
		{
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public sealed override void ConfigureServices(IServiceCollection services)
		{
			services.AddSignalR();
			services.AddDbContext<AccountContext>(options => options.UseSqlServer(Configuration["DbConnectionString"]));

			services.AddIdentity<AccountEntity, IdentityRole<Guid>>(IdentityOptionsProvider.ApplyDefaultOptions)
				.AddErrorDescriber<CodeIdentityErrorDescriber>()
				.AddEntityFrameworkStores<AccountContext>()
				.AddDefaultTokenProviders();

			services.AddSingleton<PasswordHasher<AccountEntity>>();

			base.ConfigureServices(services);
		}

		public override void ConfigureContainer(ContainerBuilder containerBuilder)
		{
			containerBuilder.RegisterModule<InternalAccountModule>();
			containerBuilder.RegisterModule<SessionModule>();

			containerBuilder.RegisterGrpcService<RegistrationService>();
			containerBuilder.RegisterGrpcService<LoginService>();
			containerBuilder.RegisterGrpcService<VerificationService>();
			containerBuilder.RegisterGrpcService<Modules.Account.Service.AccountService>();

			base.ConfigureContainer(containerBuilder);
		}

		protected override void RegisterEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
		{
			endpointRouteBuilder.MapGrpcService<IRegistrationService>();
			endpointRouteBuilder.MapGrpcService<ILoginService>();
			endpointRouteBuilder.MapGrpcService<IVerificationService>();
			endpointRouteBuilder.MapGrpcService<IAccountService>();

			base.RegisterEndpoints(endpointRouteBuilder);
		}

		protected override void SetupMassTransitBus(IContainerBuilderBusConfigurator cfg)
		{
			cfg.AddConsumer<AccountProfilePictureUpdatedConsumer>();
		}
	}
}