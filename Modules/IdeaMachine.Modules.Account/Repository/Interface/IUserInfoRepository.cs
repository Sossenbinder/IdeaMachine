using System;
using System.Threading.Tasks;

namespace IdeaMachine.Modules.Account.Repository.Interface
{
	public interface IUserInfoRepository
	{
		Task<string?> GetProfilePictureUrl(Guid userId);

		Task AddProfilePictureUrlToUser(Guid userId, string pictureUrl);
	}
}