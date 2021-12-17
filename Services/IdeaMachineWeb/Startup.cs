using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using GreenPipes;
using IdeaMachine.Common.Eventing.DI;
using IdeaMachine.Common.Grpc.DI;
using IdeaMachine.Common.IPC.DI;
using IdeaMachine.Common.Logging.Log;
using IdeaMachine.Common.RemotingProxies.Proxies;
using IdeaMachine.Common.RuntimeSerialization.DI;
using IdeaMachine.Common.SignalR;
using IdeaMachine.Common.SignalR.DI;
using IdeaMachine.Modules.Account.DI;
using IdeaMachine.Modules.Account.Events;
using IdeaMachine.Modules.Account.Service.Interface;
using IdeaMachine.Modules.Email.DI;
using IdeaMachine.Modules.Idea.DI;
using IdeaMachine.Modules.Reaction.DI;
using IdeaMachine.Modules.Session.DI;
using IdeaMachine.Service.Base.Extensions;
using IdeaMachineWeb.Controllers;
using IdeaMachineWeb.DataTypes.Validation;
using IdeaMachineWeb.Utils;
using MassTransit;
using MassTransit.SignalR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace IdeaMachineWeb
{
	public class Startup
	{
		private readonly bool _isDevelopmentEnvironment;

		public Startup(
			IConfiguration configuration,
			IWebHostEnvironment webHostEnvironment)
		{
			Configuration = configuration;
			_isDevelopmentEnvironment = webHostEnvironment.IsDevelopment();
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			ConfigureMassTransit(services);
			ConfigureIdentity(services);

			services.AddMvc(options =>
			{
				options.ModelBinderProviders.Insert(0, new DestructuringModelBinderProvider());
			});

			services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(LogProvider.CreateLogger(Configuration)));
			services.AddHttpClient<ProfilePictureController>();

			services.AddAntiforgery(x => x.HeaderName = "RequestVerificationToken");
			services.AddResponseCompression();

			services.Configure<ValidationInfo>(Configuration.GetSection("Validation"));

			if (_isDevelopmentEnvironment)
			{
				services.AddDataProtection()
					.SetApplicationName("IdeaMachineWeb")
					.PersistKeysToFileSystem(new DirectoryInfo("/keys/storage"));
			}
			else
			{
				services.AddDataProtection()
					.PersistKeysToAzureBlobStorage(Configuration["BlobStorageConnection"], "dataprotection", "keys");
			}
		}

		private void ConfigureIdentity(IServiceCollection services)
		{
			services
				.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.Events.OnRedirectToLogin = context =>
					{
						context.Response.StatusCode = 401;
						return Task.CompletedTask;
					};
				})
				.AddGoogle(options =>
				{
					options.ClientId = Configuration["GoogleClientId"];
					options.ClientSecret = Configuration["GoogleClientSecret"];
				});
		}

		private void ConfigureMassTransit(IServiceCollection services)
		{
			services.AddSignalR();

			services.AddMassTransit(x =>
			{
				x.AddSignalRHub<SignalRHub>();

				x.UsingRabbitMq((ctx, cfg) =>
				{
					cfg.Durable = false;
					cfg.AutoDelete = true;
					cfg.PurgeOnStartup = true;

					cfg.UseMessageRetry(retryConfig =>
					{
						retryConfig.Exponential(5, TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(2));
					});

					cfg.Host($"rabbitmq://{Configuration["IdeaMachine_RabbitMq"]}");

					cfg.ConfigureEndpoints(ctx);
				});

				x.AddConsumer<AccountProfilePictureUpdatedConsumer>();
			});
		}

		// ReSharper disable once UnusedMember.Global
		public void ConfigureContainer(ContainerBuilder builder)
		{
			builder.RegisterModule<IdeaModule>();
			builder.RegisterModule<EmailModule>();
			builder.RegisterModule<MassTransitModule>();
			builder.RegisterModule<AccountModule>();
			builder.RegisterModule<SessionModule>();
			builder.RegisterModule<GrpcModule>();
			builder.RegisterModule<IpcModule>();
			builder.RegisterModule<ProtobufSerializationModule>();
			builder.RegisterModule<ReactionModule>();
			builder.RegisterModule<SignalRModule>();

			builder.RegisterGrpcProxy<IRegistrationService, RegistrationServiceProxy>();
			builder.RegisterGrpcProxy<ILoginService, LoginServiceProxy>();
			builder.RegisterGrpcProxy<IVerificationService, VerificationServiceProxy>();
			builder.RegisterGrpcProxy<IAccountService, AccountServiceProxy>();

			builder.RegisterBuildCallback(lts =>
			{
				lts.Resolve<IBusControl>().Start();
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");

				endpoints.MapFallbackToController("Index", "Home");

				endpoints.MapHub<SignalRHub>("/signalRHub");
			});
		}
	}
}