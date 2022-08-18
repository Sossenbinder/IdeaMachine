using System;
using System.IO;
using Autofac;
using IdeaMachine.Modules.Account.DI;
using IdeaMachine.Modules.Account.Repository.Context;
using IdeaMachine.Modules.Account.Service.Interface;
using IdeaMachine.Modules.Session.DI;
using IdeaMachine.Service.Base.Extensions;
using IdeaMachine.Service.Base.Middleware;
using IdeaMachine.Service.Base.Startup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using Microsoft.Identity.Web;

namespace IdeaMachine.AccountService
{
	public class Startup : CommonEndpointStartup
	{
		private readonly bool _isDevelopmentEnvironment;

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

		public override void ConfigureContainer(ContainerBuilder builder)
		{
			builder.RegisterModule<InternalAccountModule>();
			builder.RegisterModule<SessionModule>();

			builder.RegisterGrpcService<Modules.Account.Service.AccountService>();

			builder.RegisterType<SessionContextMiddleware>().SingleInstance();

			base.ConfigureContainer(builder);
		}

		private void ConfigureIdentity(IServiceCollection services)
		{
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddMicrosoftIdentityWebApi(
					options =>
					{
						options.TokenValidationParameters.NameClaimType = "name";
					},
					options => Configuration.Bind("AzureAdB2C", options));
		}

		protected override void RegisterEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
		{
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

			app.UseSessionContextMiddleware();

			base.Configure(app, env);
		}
	}
}