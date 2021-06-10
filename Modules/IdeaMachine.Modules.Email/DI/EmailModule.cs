using Autofac;
using IdeaMachine.Modules.Email.Service;
using IdeaMachine.Modules.Email.Service.Interface;

namespace IdeaMachine.Modules.Email.DI
{
	public class EmailModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<EmailEventHandler>()
				.AsSelf()
				.SingleInstance()
				.AutoActivate();

			builder.RegisterType<EmailSender>()
				.As<IEmailSender>()
				.SingleInstance();
		}
	}
}