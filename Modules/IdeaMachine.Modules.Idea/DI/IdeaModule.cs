using Autofac;
using IdeaMachine.Common.Database.Extensions;
using IdeaMachine.Modules.Idea.Events;
using IdeaMachine.Modules.Idea.Events.Interface;
using IdeaMachine.Modules.Idea.Repository;
using IdeaMachine.Modules.Idea.Repository.Context;
using IdeaMachine.Modules.Idea.Repository.Interface;
using IdeaMachine.Modules.Idea.Service;
using IdeaMachine.Modules.Idea.Service.Interface;

namespace IdeaMachine.Modules.Idea.DI
{
	public class IdeaModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterDbContextFactory(ctx => new IdeaContext(ctx["DbConnectionString"]));

			builder.RegisterType<IdeaRepository>()
				.As<IIdeaRepository>()
				.SingleInstance();

			builder.RegisterType<IdeaService>()
				.As<IIdeaService>()
				.SingleInstance();

			builder.RegisterType<IdeaEvents>()
				.As<IIdeaEvents>()
				.SingleInstance();

			builder.RegisterType<IdeaRetrievalService>()
				.As<IIdeaRetrievalService>()
				.SingleInstance();
		}
	}
}