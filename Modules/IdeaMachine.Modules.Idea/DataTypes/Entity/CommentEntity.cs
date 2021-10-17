using System;
using System.ComponentModel.DataAnnotations.Schema;
using IdeaMachine.Modules.Account.DataTypes.Entity;
using IdeaMachine.Modules.Idea.DataTypes.Model;

namespace IdeaMachine.Modules.Idea.DataTypes.Entity
{
    public class CommentEntity
	{
		public int Id { get; set; }

		public int IdeaId { get; set; }

		public string Content { get; set; } = null!;

		public DateTime CreationDate { get; set; }

		public IdeaEntity Idea { get; set; } = null!;
		
		public Guid CommenterId { get; set; }
		public AccountEntity? Commenter { get; set; }
		public string? CommenterName => Commenter?.UserName;

		public CommentModel ToModel()
		{
			return new()
			{
				IdeaId = IdeaId,
				Comment = Content,
				CommenterId = CommenterId,
				CommenterName = CommenterName ?? "",
				TimeStamp = CreationDate,
				CommentId = Id,
			};
		}
	}
}
