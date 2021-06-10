using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
	}
}