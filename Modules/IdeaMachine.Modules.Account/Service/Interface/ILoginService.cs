using System.Threading.Tasks;
using IdeaMachine.Modules.Account.DataTypes.Model;

namespace IdeaMachine.Modules.Account.Service.Interface
{
	public interface ILoginService
	{
		Task Login(LoginModel loginModel);
	}
}