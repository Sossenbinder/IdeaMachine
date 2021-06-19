using Grpc.Net.Client;
using IdeaMachine.Common.IPC;

namespace IdeaMachine.Common.Grpc.Service.Interface
{
	public interface IGrpcChannelProvider
	{
		public GrpcChannel GetForEndpoint(ServiceType serviceType);
	}
}