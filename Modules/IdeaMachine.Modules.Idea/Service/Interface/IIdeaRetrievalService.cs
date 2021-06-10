using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Modules.Idea.DataTypes;

namespace IdeaMachine.Modules.Idea.Service.Interface
{
	public interface IIdeaRetrievalService
	{
		Task<List<IdeaModel>> Get();
	}
}