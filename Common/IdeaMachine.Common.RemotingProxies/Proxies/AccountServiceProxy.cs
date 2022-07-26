//----------------------
//This file was autogenerated by GrpcProxyGenerator.Service.GrpcProxyFactory
//Timestamp of generation: UTC 26/07/2022 22:27:14
//----------------------

namespace IdeaMachine.Common.RemotingProxies.Proxies
{
	public class AccountServiceProxy : IdeaMachine.Common.RemotingProxies.ProxyInvocation.AbstractDeploymentProxy<IdeaMachine.Modules.Account.Service.Interface.IAccountService>, IdeaMachine.Modules.Account.Service.Interface.IAccountService
	{
		public AccountServiceProxy(IdeaMachine.Common.Grpc.Service.Interface.IGrpcChannelProvider channelProvider)
			: base(channelProvider,
				IdeaMachine.Common.IPC.ServiceType.AccountService)
		{ }

		public System.Threading.Tasks.Task<IdeaMachine.Common.Core.Utils.IPC.ServiceResponse<System.String>> GetAccountName(
			IdeaMachine.Modules.Account.Abstractions.DataTypes.Model.GetAccountNameRequest getAccountNameModel)
			=> InvokeWithResult(service => service.GetAccountName(
				getAccountNameModel));

	}
}
