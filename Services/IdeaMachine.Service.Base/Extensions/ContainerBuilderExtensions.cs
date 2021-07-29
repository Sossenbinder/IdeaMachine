using Autofac;
using IdeaMachine.ModulesServiceBase.Interface;

namespace IdeaMachine.Service.Base.Extensions
{
	public static class ContainerBuilderExtensions
	{
		public static void RegisterGrpcService<TGrpcService>(this ContainerBuilder builder)
			where TGrpcService : class, IGrpcService
		{
			builder.RegisterType<TGrpcService>()
				.As<IGrpcService, TGrpcService>()
				.SingleInstance();
		}

		public static void RegisterGrpcProxy<TServiceInterface, TServiceImpl>(this ContainerBuilder builder)
			where TServiceImpl : TServiceInterface
			where TServiceInterface : IGrpcService
		{
			builder.RegisterType<TServiceImpl>()
				.As<IGrpcService, TServiceInterface>()
				.SingleInstance();
		}
	}
}