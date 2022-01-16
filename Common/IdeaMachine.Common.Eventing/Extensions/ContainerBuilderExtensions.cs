using Autofac;
using MassTransit;
using MassTransit.Definition;

namespace IdeaMachine.Common.Eventing.Extensions
{
	public static class ContainerBuilderExtensions
	{
		public static void RegisterConsumer<TConsumer, TConsumerDefinition>(this ContainerBuilder builder)
			where TConsumer : class, IConsumer
			where TConsumerDefinition : IConsumerDefinition<TConsumer>
		{
			builder.RegisterType<TConsumer>()
				.AsSelf()
				.InstancePerDependency();

			builder.RegisterType<TConsumerDefinition>()
				.AsSelf()
				.SingleInstance();
		}
	}
}