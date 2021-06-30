using IdeaMachine.Modules.Account.Abstractions.DataTypes.Interface;

namespace IdeaMachine.Modules.Idea.DataTypes.Events
{
	public record IdeaCreated(IUser Creator, IdeaModel Idea);
}