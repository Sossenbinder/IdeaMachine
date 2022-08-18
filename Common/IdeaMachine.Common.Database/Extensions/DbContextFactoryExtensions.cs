using System;
using Autofac;
using IdeaMachine.Common.Database.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdeaMachine.Common.Database.Extensions
{
	public static class DbContextFactoryExtensions
	{
		public static void RegisterDbContextFactory<TDbContext>(
			this IServiceCollection serviceCollection,
			Func<IConfiguration, TDbContext> contextCreator)
			where TDbContext : AbstractDbContext
		{
			serviceCollection.AddSingleton<DbContextFactory<TDbContext>>(ctx => new DbContextFactory<TDbContext>(
					ctx.GetRequiredService<IConfiguration>(),
					contextCreator));
		}

		public static void RegisterDbContextFactory<TDbContext>(
			this ContainerBuilder containerBuilder,
			Func<IConfiguration, TDbContext> contextCreator)
			where TDbContext : AbstractDbContext
		{
			containerBuilder.Register(ctx => new DbContextFactory<TDbContext>(
					ctx.Resolve<IConfiguration>(),
					contextCreator))
				.As<DbContextFactory<TDbContext>>()
				.SingleInstance();
		}
	}
}