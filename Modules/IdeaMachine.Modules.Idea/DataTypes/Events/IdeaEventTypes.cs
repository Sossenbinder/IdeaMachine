using IdeaMachine.Modules.Account.Abstractions.DataTypes.Interface;
using IdeaMachine.Modules.Idea.DataTypes.Model;

namespace IdeaMachine.Modules.Idea.DataTypes.Events
{
	public record IdeaCreated(IUser Creator, IdeaModel Idea);
}