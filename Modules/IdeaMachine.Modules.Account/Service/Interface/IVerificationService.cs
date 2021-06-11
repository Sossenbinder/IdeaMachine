using System.Threading.Tasks;
using IdeaMachine.Common.AspNetIdentity.DataTypes;
using IdeaMachine.Common.Core.Utils.IPC;

namespace IdeaMachine.Modules.Account.Service.Interface
{
	public interface IVerificationService
	{
		Task<ServiceResponse<IdentityErrorCode>> VerifyAccount(string userName, string token);
	}
}