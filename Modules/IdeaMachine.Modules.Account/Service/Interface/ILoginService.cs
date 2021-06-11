using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Account.DataTypes.Model;

namespace IdeaMachine.Modules.Account.Service.Interface
{
	public interface ILoginService
	{
		Task<ServiceResponse<LoginResult>> Login(LoginModel loginModel);
	}
}