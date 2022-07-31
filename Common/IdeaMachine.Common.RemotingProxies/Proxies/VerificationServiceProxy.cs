//----------------------
//This file was autogenerated by GrpcProxyGenerator.Service.GrpcProxyFactory
//Timestamp of generation: UTC 31/07/2022 20:00:28
//----------------------

namespace IdeaMachine.Common.RemotingProxies.Proxies
{
	public class VerificationServiceProxy : IdeaMachine.Common.RemotingProxies.ProxyInvocation.AbstractDeploymentProxy<IdeaMachine.Modules.Account.Service.Interface.IVerificationService>, IdeaMachine.Modules.Account.Service.Interface.IVerificationService
	{
		public VerificationServiceProxy(IdeaMachine.Common.Grpc.Service.Interface.IGrpcChannelProvider channelProvider)
			: base(channelProvider,
				IdeaMachine.Common.IPC.ServiceType.AccountService)
		{ }

		public System.Threading.Tasks.Task<IdeaMachine.Common.Core.Utils.IPC.ServiceResponse<IdeaMachine.Common.AspNetIdentity.DataTypes.IdentityErrorCode>> VerifyAccount(
			IdeaMachine.Modules.Account.DataTypes.Model.VerifyAccountModel verifyModel)
			=> InvokeWithResult(service => service.VerifyAccount(
				verifyModel));

	}
}
