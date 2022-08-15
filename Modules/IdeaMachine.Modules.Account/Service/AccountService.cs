using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Model;
using IdeaMachine.Modules.Account.Repository.Interface;
using IdeaMachine.Modules.Account.Service.Interface;

namespace IdeaMachine.Modules.Account.Service
{
	public class AccountService : IAccountService
	{
		private readonly IUserInfoRepository _userInfoRepository;

		public AccountService(
			IUserInfoRepository userInfoRepository)
		{
			_userInfoRepository = userInfoRepository;
		}

		public async Task<ServiceResponse<string>> GetProfilePictureUrl(GetProfilePictureUrl getProfilePictureUrl)
		{
			var profilePictureUrl = await _userInfoRepository.GetProfilePictureUrl(getProfilePictureUrl.UserIdentifier);

			return profilePictureUrl is not null ? ServiceResponse<string>.Success(profilePictureUrl) : ServiceResponse<string>.Failure();
		}
	}
}