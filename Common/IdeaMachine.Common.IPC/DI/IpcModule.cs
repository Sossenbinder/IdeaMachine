using Autofac;
using IdeaMachine.Common.IPC.Service;
using IdeaMachine.Common.IPC.Service.Interface;

namespace IdeaMachine.Common.IPC.DI
{
	public class IpcModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{

#if DEBUG
			builder.RegisterType<DockerComposeEndpointService>()
#else
			builder.RegisterType<KubernetesEndpointService>()
#endif
				.As<IEndpointService>()
				.SingleInstance();
		}
	}
}