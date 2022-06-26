using System.ServiceModel;
using System.Threading.Tasks;
using IdeaMachine.Common.AspNetIdentity.DataTypes;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Account.DataTypes.Model;
using IdeaMachine.Modules.ServiceBase.Interface;

namespace IdeaMachine.Modules.Account.Service.Interface
{
	[ServiceContract]
	public interface IVerificationService : IGrpcService
	{
		[OperationContract]
		Task<ServiceResponse<IdentityErrorCode>> VerifyAccount(VerifyAccountModel verifyModel);
	}
}