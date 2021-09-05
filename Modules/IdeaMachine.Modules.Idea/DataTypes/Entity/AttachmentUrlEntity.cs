using IdeaMachine.Modules.Idea.DataTypes.Model;

namespace IdeaMachine.Modules.Idea.DataTypes.Entity
{
	public class AttachmentUrlEntity
	{
		public int Id { get; set; }

		public string AttachmentUrl { get; set; } = null!;

		public int IdeaId { get; set; }
		public IdeaEntity Idea { get; set; } = null!;

		public AttachmentUrlModel ToModel() => new(Id, AttachmentUrl);
	}
}