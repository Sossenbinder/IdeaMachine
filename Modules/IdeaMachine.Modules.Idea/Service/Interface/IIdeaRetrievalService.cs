using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Common.IPC.DataTypes;
using IdeaMachine.Modules.Idea.DataTypes;

namespace IdeaMachine.Modules.Idea.Service.Interface
{
	public interface IIdeaRetrievalService
	{
		Task<List<IdeaModel>> Get();

		Task<List<IdeaModel>> GetForUser(Primitive<Guid> userId);

		Task<ServiceResponse<IdeaModel?>> GetSpecificIdea(Primitive<int> number);
	}
}