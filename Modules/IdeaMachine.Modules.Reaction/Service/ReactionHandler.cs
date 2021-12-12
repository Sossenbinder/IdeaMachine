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

		private readonly ISignalRService _signalRService;

		public ReactionHandler(
			IReactionEvents reactionEvents,
			IReactionRepository reactionRepository,
			ISignalRService signalRService)
		{
			_reactionRepository = reactionRepository;
			_signalRService = signalRService;

			RegisterEventHandler(reactionEvents.LikeChange, HandleLikeChange);
		}

		private async Task HandleLikeChange(LikeChange args)
		{
			var (userId, ideaId, likeState) = args;

			var putSuccess = await _reactionRepository.PutReaction(userId, ideaId, likeState);

			if (putSuccess)
			{
				await _signalRService.RaiseGroupSignalREvent(userId.ToString(), NotificationFactory.Update(ideaId, NotificationType.LikeCommited));
			}
		}
	}
}