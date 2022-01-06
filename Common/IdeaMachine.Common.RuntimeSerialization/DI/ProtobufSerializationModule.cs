using Autofac;
using IdeaMachine.Common.RuntimeSerialization.Serialize;
using IdeaMachine.Common.RuntimeSerialization.Serialize.Interface;

namespace IdeaMachine.Common.RuntimeSerialization.DI
{
	public class ProtobufSerializationModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<SerializationModelBinderService>()
				.AsSelf()
				.SingleInstance();

			builder.RegisterType<UserSessionContainerSerializer>()
				.As<ISectionSerializer>()
				.SingleInstance();

			builder.RegisterType<GrpcServiceTypeSerializer>()
				.As<ISectionSerializer>()
				.SingleInstance();
		}
	}
}