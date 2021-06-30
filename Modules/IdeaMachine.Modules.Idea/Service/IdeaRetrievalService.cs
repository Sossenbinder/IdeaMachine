using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Common.IPC.DataTypes;
using IdeaMachine.Modules.Idea.DataTypes;
using IdeaMachine.Modules.Idea.Repository.Interface;
using IdeaMachine.Modules.Idea.Service.Interface;

namespace IdeaMachine.Modules.Idea.Service
{
	public class IdeaRetrievalService : IIdeaRetrievalService
	{
		private readonly IIdeaRepository _ideaRepository;

		public IdeaRetrievalService(IIdeaRepository ideaRepository)
		{
			_ideaRepository = ideaRepository;
		}

		public async Task<List<IdeaModel>> Get()
		{
			var ideas = await _ideaRepository.Get();

			return ideas
				.Select(x => x.ToModel())
				.ToList();
		}

		public async Task<List<IdeaModel>> GetForUser(Primitive<Guid> userId)
		{
			if (userId == Guid.Empty)
			{
				return new List<IdeaModel>();
			}

			var ideas = await _ideaRepository.Get();

			return ideas
				.Select(x => x.ToModel())
				.ToList();
		}

		public async Task<ServiceResponse<IdeaModel?>> GetSpecificIdea(Primitive<int> number)
		{
			var idea = await _ideaRepository.GetSpecificIdea(number);

			return idea is null
				? ServiceResponse<IdeaModel?>.Failure()
				: ServiceResponse<IdeaModel?>.Success(idea.ToModel());
		}
	}
}