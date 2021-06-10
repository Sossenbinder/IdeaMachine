using System.Threading.Tasks;
using IdeaMachine.Common.AspNetIdentity.DataTypes;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Account.DataTypes.Model;

namespace IdeaMachine.Modules.Account.Service.Interface
{
	public interface IRegistrationService
	{
		Task<ServiceResponse<IdentityErrorCode?>> RegisterAccount(RegisterModel registerModel);
	}
}