using System;
using System.Linq;
using System.Reflection;
using Autofac;
using IdeaMachine.Common.Eventing.Abstractions.Events;
using IdeaMachine.Common.Eventing.Helper;
using IdeaMachine.Common.Eventing.MassTransit.Service;
using IdeaMachine.Common.Eventing.MassTransit.Service.Interface;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using Module = Autofac.Module;

namespace IdeaMachine.Common.Eventing.DI
{
	public class MassTransitModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<MassTransitEventingService>()
				.As<IStartable, IMassTransitEventingService>()
				.SingleInstance();

			builder.RegisterType<MassTransitSignalRService>()
				.As<ISignalRService>()
				.SingleInstance();

			builder.RegisterType<MassTransitEventFactory>()
				.AsSelf()
				.SingleInstance();

			builder.RegisterGeneric((ctx, types) =>
				{
					var type = types.First();

					var factory = ctx.Resolve<MassTransitEventFactory>();

					var method = typeof(MassTransitEventFactory)
						.GetMethods(BindingFlags.Instance | BindingFlags.Public)
						.Single(x => x.Name == nameof(MassTransitEventFactory.CreateDistinct) && x.IsGenericMethodDefinition && x.GetParameters().Length == 2);

					var createInfo = method.MakeGenericMethod(type) ?? throw new ArgumentException($"Couldn't create generic {nameof(MassTransitEventFactory.CreateDistinct)}");

					return createInfo.Invoke(factory, new object[] { null!, null! })!;
				}).As(typeof(IDistributedEvent<>))
				.SingleInstance();
		}
	}
}