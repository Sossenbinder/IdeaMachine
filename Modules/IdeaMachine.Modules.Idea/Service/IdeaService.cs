using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Eventing.DataTypes;
using IdeaMachine.Common.Eventing.Helper;
using IdeaMachine.Common.Eventing.MassTransit.Service.Interface;
using IdeaMachine.Modules.Idea.DataTypes.Events;
using IdeaMachine.Modules.Idea.DataTypes.Model;
using IdeaMachine.Modules.Idea.Events.Interface;
using IdeaMachine.Modules.Idea.Repository.Interface;
using IdeaMachine.Modules.Idea.Service.Interface;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;

namespace IdeaMachine.Modules.Idea.Service
{
	public class IdeaService : IIdeaService
	{
		private readonly IIdeaEvents _ideaEvents;

		private readonly IIdeaRepository _ideaRepository;

		private readonly INotificationService _notificationService;

		public IdeaService(
			IIdeaEvents ideaEvents,
			IIdeaRepository ideaRepository,
			INotificationService notificationService)
		{
			_ideaEvents = ideaEvents;
			_ideaRepository = ideaRepository;
			_notificationService = notificationService;
		}

		public async Task<int> Add(ISession session, IdeaModel ideaModel)
		{
			ideaModel.CreationDate = DateTime.UtcNow;

			ideaModel.Id = (await _ideaRepository.Add(ideaModel.ToEntity(session))).Id;

			await _ideaEvents.IdeaCreated.Raise(new IdeaCreated(session.User, ideaModel));

			await _notificationService.RaiseForAll(NotificationFactory.Create(ideaModel, NotificationType.Idea));

			return ideaModel.Id;
		}

		public async Task<IdeaDeleteErrorCode> Delete(ISession session, int id)
		{
			return await _ideaRepository.Delete(session.User.UserId, id);
		}
	}
}