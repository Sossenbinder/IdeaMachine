using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Common.Eventing.MassTransit.Service.Interface;
using IdeaMachine.Modules.Idea.DataTypes;
using IdeaMachine.Modules.Idea.DataTypes.Events;
using IdeaMachine.Modules.Idea.Events.Interface;
using IdeaMachine.Modules.Idea.Repository.Interface;
using IdeaMachine.Modules.Idea.Service.Interface;

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

		public async Task Add(IdeaModel ideaModel)
		{
			ideaModel.CreationDate = DateTime.UtcNow;

			await _ideaRepository.Add(ideaModel);

			await _ideaEvents.IdeaCreated.Raise(new IdeaCreated(ideaModel));
		}

		public Task<List<IdeaModel>> Get()
		{
			return _ideaRepository.Get();
		}
	}
}