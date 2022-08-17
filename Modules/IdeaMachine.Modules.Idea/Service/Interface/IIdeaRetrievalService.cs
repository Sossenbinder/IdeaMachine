using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Common.Core.Utils.Pagination;
using IdeaMachine.Common.IPC.DataTypes;
using IdeaMachine.Modules.Idea.DataTypes.Model;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;

namespace IdeaMachine.Modules.Idea.Service.Interface
{
	public interface IIdeaRetrievalService
	{
		Task<PaginationResult<int?, IdeaModel>> Get(int? paginationToken = null);

		Task<List<IdeaModel>> GetForUser(ISession session);

		Task<ServiceResponse<IdeaModel?>> GetSpecificIdea(ISession session, Primitive<int> number);
	}
}