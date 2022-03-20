using Autofac;
using Autofac.Extensions.DependencyInjection;
using IdeaMachine.Common.Eventing.DI;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace IdeaMachine.Tests.Core.DI
{
	public class TestContainerBuilderBuilder
	{
		private readonly IServiceCollection _serviceCollection;

		private readonly List<Action<ContainerBuilder>> _builderCallbacks;

		public TestContainerBuilderBuilder()
		{
			_serviceCollection = new ServiceCollection().AddLogging();
			_builderCallbacks = new List<Action<ContainerBuilder>>();
		}

		public TestContainerBuilderBuilder AddEventing()
		{
			_serviceCollection.AddMassTransitInMemoryTestHarness();
			_builderCallbacks.Add(builder => builder.RegisterModule<MassTransitModule>());
			return this;
		}

		public ContainerBuilder Create()
		{
			var cb = new ContainerBuilder();
			cb.Populate(_serviceCollection);

			foreach (var callback in _builderCallbacks)
			{
				callback(cb);
			}

			return cb;
		}
	}
}
