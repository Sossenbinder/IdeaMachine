using System;
using Autofac;
using GreenPipes;
using IdeaMachine.Common.Eventing.DI;
using IdeaMachine.Common.Grpc.DI;
using IdeaMachine.Common.Grpc.Interceptors;
using IdeaMachine.Common.IPC.DI;
using IdeaMachine.Common.Logging.Log;
using IdeaMachine.Common.RuntimeSerialization.DI;
using IdeaMachine.ModulesServiceBase.Interface;
using IdeaMachine.Service.Base.Extensions;
using MassTransit;
using MassTransit.AutofacIntegration;
using MassTransit.RabbitMqTransport;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc.Server;
using Serilog;

namespace IdeaMachine.Service.Base.Startup
{
	public abstract class CommonEndpointStartup
	{
		protected IConfiguration Configuration { get; }

		protected CommonEndpointStartup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		// .Net DI entry point
		public virtual void ConfigureServices(IServiceCollection services)
		{
			services.AddCodeFirstGrpc(grpcOptions =>
			{
				grpcOptions.EnableDetailedErrors = true;
				grpcOptions.Interceptors.Add<GrpcServiceResponseInterceptor>();
			});

			services.AddControllers();

			services.AddLogging(loggingBuilder =>
			{
				loggingBuilder.AddConsole();
				loggingBuilder.AddSerilog(LogProvider.CreateLogger(Configuration));
			});
		}

		// Autofac entry point
		public virtual void ConfigureContainer(ContainerBuilder containerBuilder)
		{
			SetupMassTransit(containerBuilder);

			containerBuilder.RegisterModule<MassTransitModule>();
			containerBuilder.RegisterModule<ProtobufSerializationModule>();
			containerBuilder.RegisterModule<GrpcModule>();
			containerBuilder.RegisterModule<IpcModule>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseRouting();
			app.UseEndpoints(RegisterEndpoints);
		}

		protected virtual void RegisterEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
		{
			endpointRouteBuilder.MapControllers();
		}

		private void SetupMassTransit(ContainerBuilder containerBuilder)
		{
			containerBuilder.AddMassTransit(configurator =>
			{
				SetupMassTransitBus(configurator);

				configurator.AddDelayedMessageScheduler();
				configurator.UsingRabbitMq((ctx, cfg) =>
				{
					SetupRabbitMq(ctx, cfg);

					cfg.UseMessageRetry(retryConfig =>
					{
						retryConfig.Exponential(5, TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(1),
							TimeSpan.FromSeconds(2));
					});

					cfg.Host($"rabbitmq://{Configuration["IdeaMachine_RabbitMq"]}");

					cfg.ConfigureEndpoints(ctx);

					cfg.UseDelayedMessageScheduler();
				});
			});

			containerBuilder.RegisterBuildCallback(x =>
			{
				x.Resolve<IBusControl>().Start();
			});
		}

		protected virtual void SetupMassTransitBus(IContainerBuilderBusConfigurator cfg)
		{

		}

		protected virtual void SetupRabbitMq(IBusRegistrationContext ctx, IRabbitMqBusFactoryConfigurator cfg)
		{

		}
	}

	/// <summary>
	/// Base template for the common case (single grpc service per microservice)
	/// All others need to derive from base
	/// </summary>
	public abstract class CommonEndpointStartup<TGrpcService> : CommonEndpointStartup
		where TGrpcService : class, IGrpcService
	{
		protected CommonEndpointStartup(IConfiguration configuration)
			: base(configuration)
		{
		}

		protected override void RegisterEndpoints(IEndpointRouteBuilder endpointRouteBuilder)
		{
			endpointRouteBuilder.MapGrpcService<TGrpcService>();

			base.RegisterEndpoints(endpointRouteBuilder);
		}

		public override void ConfigureContainer(ContainerBuilder containerBuilder)
		{
			containerBuilder.RegisterGrpcService<TGrpcService>();

			base.ConfigureContainer(containerBuilder);
		}
	}
}