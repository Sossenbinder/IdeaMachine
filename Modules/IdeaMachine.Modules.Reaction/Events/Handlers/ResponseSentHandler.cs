using System.Threading.Tasks;
using IdeaMachine.Common.Database.Context;
using IdeaMachine.Common.Eventing.DataTypes;
using IdeaMachine.Common.Eventing.Helper;
using IdeaMachine.Common.Eventing.MassTransit.Service.Interface;
using IdeaMachine.Modules.Idea.Repository.Context;
using IdeaMachine.Modules.Reaction.DataTypes.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdeaMachine.Modules.Reaction.Events.Handlers
{
	public class ResponseSentHandler : IConsumer<ResponseSent>
	{
		private readonly ILogger<ResponseSentHandler> _logger;

		private readonly DbContextFactory<IdeaContext> _ideaDbContextFactory;

		private readonly INotificationService _notificationService;

		public ResponseSentHandler(
			ILogger<ResponseSentHandler> logger,
			DbContextFactory<IdeaContext> dbContextFactory,
			INotificationService notificationService)
		{
			_logger = logger;
			_ideaDbContextFactory = dbContextFactory;
			_notificationService = notificationService;
		}

		public async Task Consume(ConsumeContext<ResponseSent> context)
		{
			var (userId, ideaId, _) = context.Message;

			await using var ctx = _ideaDbContextFactory.CreateDbContext();

			var owner = (await ctx
				.Ideas
				.Include(x => x.Creator)
				.FirstOrDefaultAsync(x => x.Id == ideaId))?.Creator;

			if (owner is null)
			{
				_logger.LogError($"{userId} responded to idea with id {ideaId}, but no owner could be determined");
				return;
			}

			await _notificationService.RaiseForUser(owner.Id, NotificationFactory.Create(context.Message, NotificationType.IdeaResponse));
		}
	}
}