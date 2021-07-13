using Autofac;

namespace IdeaMachine.Common.RuntimeSerialization.DI
{
	public class ProtobufSerializationModule : Module
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