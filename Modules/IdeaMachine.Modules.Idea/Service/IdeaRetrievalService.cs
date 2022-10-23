using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Common.Core.Utils.Pagination;
using IdeaMachine.Common.IPC.DataTypes;
using IdeaMachine.Modules.Idea.DataTypes.Model;
using IdeaMachine.Modules.Idea.Repository.Interface;
using IdeaMachine.Modules.Idea.Service.Interface;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;

namespace IdeaMachine.Modules.Idea.Service
{
	public class IdeaRetrievalService : IIdeaRetrievalService
	{
		private readonly IIdeaRepository _ideaRepository;

		public IdeaRetrievalService(IIdeaRepository ideaRepository)
		{
			_ideaRepository = ideaRepository;
		}

		public Task<int> CountAll(Guid? userId)
		{
			return _ideaRepository.Count(userId);
		}

		public async Task<PaginationResult<int?, IdeaModel>> Get(int? paginationToken = null)
		{
			var ideaResult = await _ideaRepository.Get(paginationToken);

			return ideaResult.WithNewPayload(ideaResult.Data.Select(x => x.ToModel()));
		}

		public async Task<List<IdeaModel>> GetForUser(ISession session)
		{
			if (session.User.UserId == Guid.Empty)
			{
				return new List<IdeaModel>();
			}

			var ideas = await _ideaRepository.Get();

			return ideas
				.Data
				.Select(x => x.ToModel())
				.ToList();
		}

		public async Task<ServiceResponse<IdeaModel?>> GetSpecificIdea(ISession session, Primitive<int> number)
		{
			var idea = await _ideaRepository.GetSpecificIdea(number);

			if (idea is null)
			{
				return ServiceResponse<IdeaModel?>.Failure();
			}

			return ServiceResponse<IdeaModel?>.Success(idea.ToModel());
		}
	}
}