using System.Threading.Tasks;
using IdeaMachine.Modules.Idea.DataTypes.Model;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;

namespace IdeaMachine.Modules.Idea.Service.Interface
{
	public interface IIdeaService
	{
		Task<int> Add(ISession session, IdeaModel ideaModel);

		Task<IdeaDeleteErrorCode> Delete(ISession session, int id);
	}
}