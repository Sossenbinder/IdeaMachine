using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;

namespace IdeaMachine.Modules.Idea.DataTypes.Events
{
	public record IdeaCreated(IUserSession Creator, IdeaModel Idea);
}