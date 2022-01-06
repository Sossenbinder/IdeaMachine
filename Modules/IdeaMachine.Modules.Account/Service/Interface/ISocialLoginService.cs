using System.ServiceModel;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Account.DataTypes.Model;
using IdeaMachine.ModulesServiceBase.Attributes;
using IdeaMachine.ModulesServiceBase.Interface;

namespace IdeaMachine.Modules.Account.Service.Interface
{
	[ServiceContract]
	[GrpcServiceIdentifier(4)]
	public interface ISocialLoginService : IGrpcService
	{
		[OperationContract]
		Task<ServiceResponse> AddExternalUser(SocialLoginInformation loginInfo);
	}
}