using System;
using System.Linq;
using System.Reflection;
using Autofac;
using IdeaMachine.Common.Eventing.Abstractions.Events;
using IdeaMachine.Common.Eventing.Helper;
using IdeaMachine.Common.Eventing.MassTransit.Service;
using IdeaMachine.Common.Eventing.MassTransit.Service.Interface;

namespace IdeaMachine.Common.Eventing.DI
{
	public class MassTransitModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<MassTransitEventingService>()
				.As<IStartable, IMassTransitEventingService>()
				.SingleInstance();

			builder.RegisterType<MassTransitSignalRBackplaneService>()
				.As<IMassTransitSignalRBackplaneService>()
				.SingleInstance();

			builder.RegisterType<MassTransitEventFactory>()
				.AsSelf()
				.SingleInstance();

			builder.RegisterGeneric((ctx, types, parameters) =>
				{
					var type = types.First();

					var factory = ctx.Resolve<MassTransitEventFactory>();

					MethodInfo create = typeof(MassTransitEventFactory).GetMethod(nameof(MassTransitEventFactory.Create), new Type[0])?.MakeGenericMethod(type) ?? throw new ArgumentException($"Couldn't create generic {nameof(MassTransitEventFactory.Create)}");

					return create.Invoke(factory, new object[0])!;
				}).As(typeof(IDistributedEvent<>))
				.SingleInstance();
		}
	}
}