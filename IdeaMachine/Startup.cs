using System;
using System.Data.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using Autofac;
using GreenPipes;
using GreenPipes.Configurators;
using IdeaMachine.Common.AspNetIdentity.Helper;
using IdeaMachine.Common.Eventing.DI;
using IdeaMachine.Common.Logging.Log;
using IdeaMachine.Common.SignalR;
using IdeaMachine.Modules.Account.DataTypes.Entity;
using IdeaMachine.Modules.Account.DI;
using IdeaMachine.Modules.Account.Repository.Context;
using IdeaMachine.Modules.Email.DI;
using IdeaMachine.Modules.Idea.DI;
using MassTransit;
using MassTransit.SignalR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdeaMachine
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
			services.AddControllersWithViews();

			services.AddSignalR();

			services.AddMemoryCache();

			services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(LogProvider.CreateLogger(Configuration)));

			services.AddAntiforgery(x => x.HeaderName = "RequestVerificationToken");

			services.AddResponseCompression();

			RegisterIdentity(services);

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
			});

			if (_isDevelopmentEnvironment)
			{
				services.AddDataProtection()
					.SetApplicationName("IdeaMachine")
					.PersistKeysToFileSystem(new DirectoryInfo("/keys/storage"));
			}
			else
			{
				//services.AddDataProtection()
				//	.PersistKeysToAzureBlobStorage(Configuration["CloudStorageAccountConnectionString"], "dataprotection", "keys")
				//	.ProtectKeysWithAzureKeyVault(new Uri("https://picro.vault.azure.net/"), new DefaultAzureCredential());
			}
		}

		private void RegisterIdentity(IServiceCollection services)
		{
			services.AddDbContext<AccountContext>(options => options.UseNpgsql(Configuration["PostgresConnectionString"]));

			services.AddIdentity<AccountEntity, IdentityRole<int>>(IdentityOptionsProvider.ApplyDefaultOptions)
				.AddErrorDescriber<CodeIdentityErrorDescriber>()
				.AddEntityFrameworkStores<AccountContext>()
				.AddDefaultTokenProviders();

			services.AddSingleton<PasswordHasher<AccountEntity>>();
		}

		// ReSharper disable once UnusedMember.Global
		public void ConfigureContainer(ContainerBuilder builder)
		{
			builder.RegisterModule<IdeaModule>();
			builder.RegisterModule<EmailModule>();
			builder.RegisterModule<MassTransitModule>();
			builder.RegisterModule<AccountModule>();
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

			app.UseAuthorization();
			app.UseAuthentication();

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