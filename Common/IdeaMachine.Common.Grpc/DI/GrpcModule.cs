using Autofac;
using IdeaMachine.Common.Grpc.Service;
using IdeaMachine.Common.Grpc.Service.Interface;

namespace IdeaMachine.Common.Grpc.DI
{
	public class GrpcModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<GrpcChannelProvider>()
				.As<IGrpcChannelProvider>()
				.SingleInstance();
		}
	}
}