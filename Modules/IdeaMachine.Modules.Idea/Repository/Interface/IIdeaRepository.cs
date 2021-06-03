using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Modules.Idea.DataTypes;

namespace IdeaMachine.Modules.Idea.Repository.Interface
{
	public interface IIdeaRepository
	{
		Task Add(IdeaModel idea);

		Task<List<IdeaModel>> Get();
	}
}