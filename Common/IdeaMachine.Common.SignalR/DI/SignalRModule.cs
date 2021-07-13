using Autofac;
using IdeaMachine.Common.SignalR.Service;
using IdeaMachine.Common.SignalR.Service.Interface;

namespace IdeaMachine.Common.SignalR.DI
{
	public class SignalRModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ClientTrackingService>()
				.As<IClientTrackingService>()
				.SingleInstance();
		}
	}
}