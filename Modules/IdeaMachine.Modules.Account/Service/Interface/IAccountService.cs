using System.ServiceModel;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Model;
using IdeaMachine.ModulesServiceBase.Interface;

namespace IdeaMachine.Modules.Account.Service.Interface
{
	[ServiceContract]
    public interface IAccountService : IGrpcService
	{
		[OperationContract]
		Task<ServiceResponse<string>> GetAccountName(GetAccountNameRequest getAccountNameModel);
    }
}
