using System;
using System.Collections.Generic;
using Autofac;
using IdeaMachine.Common.DI.DataTypes;
using MassTransit;

namespace IdeaMachine.Common.Eventing.Extensions
{
	public static class IComponentContextExtensions
	{
		public static void RegisterMassTransitConsumers(this IReceiveEndpointConfigurator ec, IComponentContext context)
		{
			var localContext = context.Resolve<IComponentContext>();
			var consumers = localContext.Resolve<IEnumerable<DataContainer<Type>>>();

			foreach (var consumer in consumers)
			{
				ec.Consumer(consumer.Data, localContext.Resolve);
			}
		}
	}
}