using Autofac;
using IdeaMachine.Common.Database.Extensions;
using IdeaMachine.Modules.Reaction.Events;
using IdeaMachine.Modules.Reaction.Events.Interface;
using IdeaMachine.Modules.Reaction.Repository;
using IdeaMachine.Modules.Reaction.Repository.Context;
using IdeaMachine.Modules.Reaction.Repository.Interface;
using IdeaMachine.Modules.Reaction.Service;
using IdeaMachine.Modules.Reaction.Service.Interface;

namespace IdeaMachine.Modules.Reaction.DI
{
	public class ReactionModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ReactionEvents>()
				.As<IReactionEvents>()
				.SingleInstance();

			builder.RegisterType<ReactionHandler>()
				.AsSelf()
				.SingleInstance()
				.AutoActivate();

			builder.RegisterDbContextFactory(ctx => new ReactionContext(ctx["DbConnectionString"]));

			builder.RegisterType<ReactionRepository>()
				.As<IReactionRepository>()
				.SingleInstance();

			builder.RegisterType<ReactionRetrievalService>()
				.As<IReactionRetrievalService>()
				.SingleInstance();
		}
	}
}