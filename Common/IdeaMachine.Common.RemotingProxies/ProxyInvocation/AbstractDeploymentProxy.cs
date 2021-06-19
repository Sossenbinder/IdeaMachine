using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Grpc.Service.Interface;
using IdeaMachine.Common.IPC;
using IdeaMachine.Common.RemotingProxies.ProxyInvocation.Interface;
using IdeaMachine.ModulesServiceBase.Interface;
using ProtoBuf.Grpc.Client;

namespace IdeaMachine.Common.RemotingProxies.ProxyInvocation
{
	/// <summary>
	/// Serves as the base for all proxy services, taking care of load balancing for all the implementers
	/// </summary>
	/// <typeparam name="TServiceType"></typeparam>
	public abstract class AbstractDeploymentProxy<TServiceType> : IDeploymentProxy<TServiceType>
		where TServiceType : class, IGrpcService
	{
		private readonly IGrpcChannelProvider _channelProvider;

		private readonly ServiceType _serviceType;

		protected AbstractDeploymentProxy(
			IGrpcChannelProvider channelProvider,
			ServiceType serviceType)
		{
			_channelProvider = channelProvider;

			_serviceType = serviceType;
		}

		public async Task Invoke(Func<TServiceType, Task> invocationFunc)
		{
			var channel = _channelProvider.GetForEndpoint(_serviceType);

			var client = channel.CreateGrpcService<TServiceType>();

			await invocationFunc(client);
		}

		public async Task<TResult> InvokeWithResult<TResult>(Func<TServiceType, Task<TResult>> invocationFunc)
		{
			var channel = _channelProvider.GetForEndpoint(_serviceType);

			var client = channel.CreateGrpcService<TServiceType>();

			return await invocationFunc(client);
		}
	}
}