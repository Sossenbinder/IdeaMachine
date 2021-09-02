namespace IdeaMachine.Modules.Idea.DataTypes.Entity
{
	public class TagEntity
	{
		public int Id { get; set; }

		public string Tag { get; set; } = null!;

		public int IdeaId { get; set; }
		public IdeaEntity Idea { get; set; } = null!;
	}
}