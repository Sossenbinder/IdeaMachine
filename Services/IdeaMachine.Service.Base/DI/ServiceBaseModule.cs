using Autofac;
using IdeaMachine.Service.Base.Serialization;

namespace IdeaMachine.Service.Base.DI
{
	public class ServiceBaseModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<GrpcServiceTypeSerializer>()
				.AsSelf()
				.SingleInstance();

			builder.RegisterType<SerializationModelBinderService>()
				.SingleInstance()
				.AutoActivate();

			builder.RegisterType<SerializationHelper>()
				.AsSelf()
				.SingleInstance();
		}
	}
}