using System.ServiceModel;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Account.DataTypes.Model;
using IdeaMachine.Modules.ServiceBase.Interface;

namespace IdeaMachine.Modules.Account.Service.Interface
{
	[ServiceContract]
	public interface ILoginService : IGrpcService
	{
		[OperationContract]
		Task<ServiceResponse<LoginResult>> Login(LoginModel loginModel);

		[OperationContract]
		Task RefreshLogin(RefreshLoginModel refreshLoginModel);
	}
}