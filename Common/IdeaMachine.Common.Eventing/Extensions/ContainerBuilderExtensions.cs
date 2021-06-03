using System;
using Autofac;
using IdeaMachine.Common.DI.DataTypes;
using MassTransit;

namespace IdeaMachine.Common.Eventing.Extensions
{
	public static class ContainerBuilderExtensions
	{
		public static void RegisterMtConsumer<TConsumer>(this ContainerBuilder builder)
			where TConsumer : IConsumer
		{
			builder.RegisterInstance(DataContainer<Type>.Create(typeof(TConsumer)))
				.As<DataContainer<Type>>()
				.SingleInstance();

			builder.RegisterType<TConsumer>()
				.AsSelf()
				.InstancePerDependency();
		}
	}
}