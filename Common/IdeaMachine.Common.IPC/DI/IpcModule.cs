using Autofac;
using IdeaMachine.Common.IPC.Service;
using IdeaMachine.Common.IPC.Service.Interface;

namespace IdeaMachine.Common.IPC.DI
{
	public class IpcModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<DockerComposeEndpointService>()
				.As<IEndpointService>()
				.SingleInstance();
		}
	}
}