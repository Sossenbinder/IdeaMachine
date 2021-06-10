using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Modules.Idea.DataTypes.Entity;

namespace IdeaMachine.Modules.Idea.Repository.Interface
{
	public interface IIdeaRepository
	{
		Task<IdeaEntity> Add(IdeaEntity idea);

		Task<List<IdeaEntity>> Get();
	}
}