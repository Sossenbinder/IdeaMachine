using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core.Resolving.Pipeline;
using IdeaMachine.Common.Eventing.Abstractions.Events;
using IdeaMachine.Common.Eventing.Events;
using IdeaMachine.Common.Eventing.Helper;
using IdeaMachine.Common.Eventing.MassTransit.Service;
using IdeaMachine.Common.Eventing.MassTransit.Service.Interface;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Events;

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

			builder.RegisterGeneric((ctx, types) =>
				{
					var type = types.First();

					var factory = ctx.Resolve<MassTransitEventFactory>();

					MethodInfo create = typeof(MassTransitEventFactory).GetMethod(nameof(MassTransitEventFactory.CreateRegular), new Type[0])?.MakeGenericMethod(type) ?? throw new ArgumentException($"Couldn't create generic {nameof(MassTransitEventFactory.CreateDistinct)}");

					return create.Invoke(factory, new object[0])!;
				}).As(typeof(IDistributedEvent<>))
				.SingleInstance();
		}
	}
}