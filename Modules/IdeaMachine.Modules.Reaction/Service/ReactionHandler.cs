using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Eventing.DataTypes;
using IdeaMachine.Common.Eventing.Helper;
using IdeaMachine.Common.Eventing.MassTransit.Service.Interface;
using IdeaMachine.Modules.Reaction.DataTypes.Events;
using IdeaMachine.Modules.Reaction.Events.Interface;
using IdeaMachine.Modules.Reaction.Repository.Interface;
using IdeaMachine.ModulesServiceBase;
using MassTransit;

namespace IdeaMachine.Modules.Reaction.Service
{
	public class ReactionHandler : ServiceBaseWithoutLogger
	{
		private readonly IReactionRepository _reactionRepository;

		private readonly INotificationService _notificationService;

		public ReactionHandler(
			IReactionEvents reactionEvents,
			IReactionRepository reactionRepository,
			INotificationService notificationService)
		{
			_reactionRepository = reactionRepository;
			_notificationService = notificationService;

			RegisterEventHandler(reactionEvents.LikeChange, HandleLikeChange);
		}

		private async Task HandleLikeChange(LikeChange args)
		{
			var (userId, ideaId, likeState) = args;

			var putSuccess = await _reactionRepository.PutReaction(userId, ideaId, likeState);

			if (putSuccess)
			{
				await _notificationService.RaiseForGroup(userId.ToString(), NotificationFactory.Update(ideaId, NotificationType.LikeCommited));
			}
		}
	}
}