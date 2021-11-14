using System.ServiceModel;
using System.Threading.Tasks;
using IdeaMachine.ModulesServiceBase.Interface;

namespace IdeaMachine.Modules.Account.Service.Interface
{
	[ServiceContract]
    public interface IAccountService : IGrpcService
	{
		[OperationContract]
		Task<Abstractions.DataTypes.Model.GetAccountName.Response> GetAccountName(Abstractions.DataTypes.Model.GetAccountName.Request getAccountNameModel);
    }
}
