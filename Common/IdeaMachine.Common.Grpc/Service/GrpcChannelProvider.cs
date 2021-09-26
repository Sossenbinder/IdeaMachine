using System.Collections.Concurrent;
using Grpc.Net.Client;
using IdeaMachine.Common.Grpc.Service.Interface;
using IdeaMachine.Common.IPC;
using IdeaMachine.Common.IPC.Service.Interface;
using ProtoBuf.Grpc.Client;

namespace IdeaMachine.Common.Grpc.Service
{
	public class GrpcChannelProvider : IGrpcChannelProvider
	{
		private const int Port = 11337;

		private readonly IEndpointService _endpointService;

		private readonly ConcurrentDictionary<ServiceType, GrpcChannel> _channelCache;

		public GrpcChannelProvider(IEndpointService endpointService)
		{
			_endpointService = endpointService;
			_channelCache = new ConcurrentDictionary<ServiceType, GrpcChannel>();

			GrpcClientFactory.AllowUnencryptedHttp2 = true;
		}

		public GrpcChannel GetForEndpoint(ServiceType serviceType)
		{
			return _channelCache.GetOrAdd(serviceType, CreateChannel);
		}

		private GrpcChannel CreateChannel(ServiceType serviceType)
		{
			var dns = _endpointService.GetStatelessEndpointDomainName(serviceType);

			return GrpcChannel.ForAddress($"http://{dns}:{Port}");
		}
	}
}