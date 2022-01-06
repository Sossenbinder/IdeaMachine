using System.ServiceModel;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Account.DataTypes.Model;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;
using IdeaMachine.ModulesServiceBase.Attributes;
using IdeaMachine.ModulesServiceBase.Interface;

namespace IdeaMachine.Modules.Account.Service.Interface
{
	[ServiceContract]
	[GrpcServiceIdentifier(2)]
	public interface ILoginService : IGrpcService
	{
		[OperationContract]
		Task<ServiceResponse<LoginResult>> Login(LoginModel loginModel);

		[OperationContract]
		Task Logout(ISession session);

		[OperationContract]
		Task RefreshLogin(RefreshLoginModel refreshLoginModel);
	}
}