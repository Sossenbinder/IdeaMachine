using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Common.Core.Utils.Pagination;
using IdeaMachine.Common.IPC.DataTypes;
using IdeaMachine.Modules.Idea.DataTypes;
using IdeaMachine.Modules.Idea.DataTypes.Model;

namespace IdeaMachine.Modules.Idea.Service.Interface
{
	public interface IIdeaRetrievalService
	{
		Task<PaginationResult<int?, IdeaModel>> Get(int? paginationToken = null);

		Task<List<IdeaModel>> GetForUser(Primitive<Guid> userId);

		Task<ServiceResponse<IdeaModel?>> GetSpecificIdea(Primitive<int> number);
	}
}