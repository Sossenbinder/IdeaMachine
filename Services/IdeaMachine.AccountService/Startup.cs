using System;
using System.IO;
using Autofac;
using IdeaMachine.Common.AspNetIdentity.Extension;
using IdeaMachine.Common.AspNetIdentity.Helper;
using IdeaMachine.Modules.Account.DataTypes.Entity;
using IdeaMachine.Modules.Account.DI;
using IdeaMachine.Modules.Account.Repository.Context;
using IdeaMachine.Modules.Account.Service;
using IdeaMachine.Modules.Account.Service.Interface;
using IdeaMachine.Modules.Session.DI;
using IdeaMachine.Service.Base.Extensions;
using IdeaMachine.Service.Base.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdeaMachine.AccountService
{
	public class Startup : CommonEndpointStartup
	{
		private bool _isDevelopmentEnvironment;

		public Startup(
			IConfiguration configuration,
			IWebHostEnvironment webHostEnvironment)
			: base(configuration)
		{
			_isDevelopmentEnvironment = webHostEnvironment.IsDevelopment();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public override sealed void ConfigureServices(IServiceCollection services)
		{
			services.Configure<ForwardedHeadersOptions>(options =>
			{
				options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
				options.KnownNetworks.Clear();
				options.KnownProxies.Clear();
			});
			ConfigureIdentity(services);

			if (_isDevelopmentEnvironment)
			{
				services.AddDataProtection()
					.SetApplicationName("IdeaMachine")
					.PersistKeysToFileSystem(new DirectoryInfo("/keys/storage"));
			}
			else
			{
				services.AddDataProtection()
					.PersistKeysToAzureBlobStorage(Configuration["BlobStorageConnection"], "dataprotection", "keys");
			}

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

		private void ConfigureIdentity(IServiceCollection services)
		{
			services.AddDbContext<AccountContext>(options => options.UseSqlServer(Configuration["DbConnectionString"]));
			services.AddIdentityWithoutDefaultAuthSchemes<AccountEntity, IdentityRole<Guid>>()
				.AddEntityFrameworkStores<AccountContext>();

			var authBuilder = services.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
					options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
					options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
				})
				.AddCookie(IdentityConstants.ApplicationScheme, o =>
				{
					o.LoginPath = new PathString("/Logon/login");
				});

			authBuilder.AddExternalCookie();
			authBuilder.AddTwoFactorRememberMeCookie();
			authBuilder.AddTwoFactorUserIdCookie();
			authBuilder.AddGoogle(options =>
			{
				options.ClientId = Configuration["GoogleClientId"];
				options.ClientSecret = Configuration["GoogleClientSecret"];
				options.SignInScheme = IdentityConstants.ExternalScheme;
			});
		}

		protected override void RegisterEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
		{
			endpointRouteBuilder.MapGrpcService<IRegistrationService>();
			endpointRouteBuilder.MapGrpcService<ILoginService>();
			endpointRouteBuilder.MapGrpcService<IVerificationService>();
			endpointRouteBuilder.MapGrpcService<IAccountService>();

			endpointRouteBuilder.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			base.RegisterEndpoints(endpointRouteBuilder);
		}

		public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseAuthentication();
			app.UseAuthorization();

			base.Configure(app, env);
		}
	}
}