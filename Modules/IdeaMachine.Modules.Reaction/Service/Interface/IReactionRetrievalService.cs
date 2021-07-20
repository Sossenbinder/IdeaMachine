using System;
using System.Threading.Tasks;
using IdeaMachine.Modules.Idea.Abstractions.DataTypes.Model;

namespace IdeaMachine.Modules.Reaction.Service.Interface
{
	public interface IReactionRetrievalService
	{
		Task<IdeaReactionMetaData> GetIdeaReactionMetaData(int ideaId, Guid userId);
	}
}