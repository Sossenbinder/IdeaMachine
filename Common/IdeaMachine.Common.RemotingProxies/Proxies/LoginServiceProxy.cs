//----------------------
//This file was autogenerated by GrpcProxyGenerator.Service.GrpcProxyFactory
//Timestamp of generation: UTC 10/02/2022 23:01:50
//----------------------

namespace IdeaMachine.Common.RemotingProxies.Proxies
{
	public class LoginServiceProxy : IdeaMachine.Common.RemotingProxies.ProxyInvocation.AbstractDeploymentProxy<IdeaMachine.Modules.Account.Service.Interface.ILoginService>, IdeaMachine.Modules.Account.Service.Interface.ILoginService
	{
		public LoginServiceProxy(IdeaMachine.Common.Grpc.Service.Interface.IGrpcChannelProvider channelProvider)
			: base(channelProvider,
				IdeaMachine.Common.IPC.ServiceType.AccountService)
		{ }

		public System.Threading.Tasks.Task<IdeaMachine.Common.Core.Utils.IPC.ServiceResponse<IdeaMachine.Modules.Account.DataTypes.Model.LoginResult>> Login(
			IdeaMachine.Modules.Account.DataTypes.Model.LoginModel loginModel)
			=> InvokeWithResult(service => service.Login(
				loginModel));

		public System.Threading.Tasks.Task Logout(
			IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface.ISession session)
			=> Invoke(service => service.Logout(
				session));

		public System.Threading.Tasks.Task RefreshLogin(
			IdeaMachine.Modules.Account.DataTypes.Model.RefreshLoginModel refreshLoginModel)
			=> Invoke(service => service.RefreshLogin(
				refreshLoginModel));

	}
}
