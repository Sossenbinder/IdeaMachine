using System;
using Autofac;
using IdeaMachine.Common.AspNetIdentity.Helper;
using IdeaMachine.Modules.Account.DataTypes.Entity;
using IdeaMachine.Modules.Account.DI;
using IdeaMachine.Modules.Account.Repository.Context;
using IdeaMachine.Modules.Account.Service;
using IdeaMachine.Modules.Account.Service.Interface;
using IdeaMachine.Service.Base.Extensions;
using IdeaMachine.Service.Base.Startup;
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

			services.AddAuthorization(options =>
			{
				options.AddPolicy("Blub", policy => policy.RequireClaim(""));
			});

			services.AddSingleton<PasswordHasher<AccountEntity>>();

			base.ConfigureServices(services);
		}

		public override void ConfigureContainer(ContainerBuilder containerBuilder)
		{
			containerBuilder.RegisterModule<InternalAccountModule>();

			containerBuilder.RegisterGrpcService<RegistrationService>();
			containerBuilder.RegisterGrpcService<LoginService>();
			containerBuilder.RegisterGrpcService<VerificationService>();

			base.ConfigureContainer(containerBuilder);
		}

		protected override void RegisterEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
		{
			endpointRouteBuilder.MapGrpcService<IRegistrationService>();
			endpointRouteBuilder.MapGrpcService<ILoginService>();
			endpointRouteBuilder.MapGrpcService<IVerificationService>();

			base.RegisterEndpoints(endpointRouteBuilder);
		}
	}
}