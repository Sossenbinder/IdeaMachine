//----------------------
//This file was autogenerated by GrpcProxyGenerator.Service.GrpcProxyFactory
//Timestamp of generation: UTC 10/01/2022 20:17:58
//----------------------

namespace IdeaMachine.Common.RemotingProxies.Proxies
{
	public class RegistrationServiceProxy : IdeaMachine.Common.RemotingProxies.ProxyInvocation.AbstractDeploymentProxy<IdeaMachine.Modules.Account.Service.Interface.IRegistrationService>, IdeaMachine.Modules.Account.Service.Interface.IRegistrationService
	{
		public RegistrationServiceProxy(IdeaMachine.Common.Grpc.Service.Interface.IGrpcChannelProvider channelProvider)
			: base(channelProvider,
				IdeaMachine.Common.IPC.ServiceType.AccountService)
		{ }

		public System.Threading.Tasks.Task<IdeaMachine.Common.Core.Utils.IPC.ServiceResponse<IdeaMachine.Common.AspNetIdentity.DataTypes.IdentityErrorCode>> RegisterAccount(
			IdeaMachine.Modules.Account.DataTypes.Model.RegisterModel registerModel)
			=> InvokeWithResult(service => service.RegisterAccount(
				registerModel));

	}
}
