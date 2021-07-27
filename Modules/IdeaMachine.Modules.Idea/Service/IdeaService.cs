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

		private readonly IMassTransitSignalRBackplaneService _massTransitSignalRBackplaneService;

		public IdeaService(
			IIdeaEvents ideaEvents,
			IIdeaRepository ideaRepository,
			IMassTransitSignalRBackplaneService massTransitSignalRBackplaneService)
		{
			_ideaEvents = ideaEvents;
			_ideaRepository = ideaRepository;
			_massTransitSignalRBackplaneService = massTransitSignalRBackplaneService;
		}

		public async Task Add(ISession session, IdeaModel ideaModel)
		{
			ideaModel.CreationDate = DateTime.UtcNow;

			ideaModel.Id = (await _ideaRepository.Add(ideaModel.ToEntity(session))).Id;

			await _ideaEvents.IdeaCreated.Raise(new IdeaCreated(session.User, ideaModel));

			await _massTransitSignalRBackplaneService.RaiseAllSignalREvent(NotificationFactory.Create(ideaModel, NotificationType.Idea));
		}

		public async Task<IdeaDeleteErrorCode> Delete(ISession session, int id)
		{
			return await _ideaRepository.Delete(session.User.UserId, id);
		}
	}
}