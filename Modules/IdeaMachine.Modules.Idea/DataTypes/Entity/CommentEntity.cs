using System;
using IdeaMachine.Modules.Idea.DataTypes.Model;

namespace IdeaMachine.Modules.Idea.DataTypes.Entity
{
    public class CommentEntity
	{
		public int Id { get; set; }

		public Guid CommenterId { get; set; }

		public int IdeaId { get; set; }

		public string Content { get; set; } = null!;

		public DateTime CreationDate { get; set; }

		public IdeaEntity Idea { get; set; } = null!;

		public CommentModel ToModel()
		{
			return new()
			{
				IdeaId = IdeaId,
				Comment = Content,
				CommenterId = CommenterId,
				TimeStamp = CreationDate,
				CommentId = Id,
			};
		}
	}
}
