using System.Threading.Tasks;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Events;
using IdeaMachine.Modules.Idea.Repository.Interface;
using IdeaMachine.Modules.ServiceBase;

namespace IdeaMachine.Modules.Idea.Service
{
	public class IdeaMigrationService : ServiceBaseWithoutLogger
	{
		private readonly IIdeaRepository _ideaRepository;

		public IdeaMigrationService(IIdeaRepository ideaRepository)
		{
			_ideaRepository = ideaRepository;
		}

		private async Task OnAccountVerified(AccountVerified accountVerifiedInfo)
		{
			var (oldUser, newUser) = accountVerifiedInfo;

			await _ideaRepository.MigrateIdeas(oldUser.UserId, newUser.UserId);
		}
	}
}