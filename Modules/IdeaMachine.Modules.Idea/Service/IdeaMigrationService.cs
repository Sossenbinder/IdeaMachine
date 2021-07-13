using System.Threading.Tasks;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Events;
using IdeaMachine.Modules.Account.Abstractions.Events.Interface;
using IdeaMachine.Modules.Idea.Repository.Interface;
using IdeaMachine.ModulesServiceBase;

namespace IdeaMachine.Modules.Idea.Service
{
	public class IdeaMigrationService : ServiceBaseWithoutLogger
	{
		private readonly IIdeaRepository _ideaRepository;

		public IdeaMigrationService(
			IAccountEvents accountEvents,
			IIdeaRepository ideaRepository)
		{
			_ideaRepository = ideaRepository;
			RegisterEventHandler(accountEvents.AccountVerified, OnAccountVerified);
		}

		private async Task OnAccountVerified(AccountVerified accountVerifiedInfo)
		{
			var (oldUser, newUser) = accountVerifiedInfo;

			await _ideaRepository.MigrateIdeas(oldUser.UserId, newUser.UserId);
		}
	}
}