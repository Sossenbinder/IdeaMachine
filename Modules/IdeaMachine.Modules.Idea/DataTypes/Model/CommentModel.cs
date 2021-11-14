using System;
using IdeaMachine.Modules.Idea.DataTypes.Entity;
using IdeaMachine.Modules.Idea.DataTypes.UiModel;

namespace IdeaMachine.Modules.Idea.DataTypes.Model
{
	public class CommentModel
	{
		public int CommentId { get; set; }

		public Guid CommenterId {  get; set; }

		public string CommenterName { get; set; }

		public int IdeaId { get; set; }

		public string Comment { get; set; } = null!;

		public DateTime TimeStamp { get; set; }

		public CommentEntity ToEntity()
		{
			return new CommentEntity()
			{
				CommenterId = CommenterId,
				Content = Comment,
				CreationDate = TimeStamp,
				IdeaId = IdeaId,
			};
		}

		public CommentUiModel ToUiModel()
		{
			return new CommentUiModel()
			{
				Comment = Comment,
				CommenterId = CommenterId,
				CommenterName = CommenterName,
				Id = CommentId,
				IdeaId = IdeaId,
				TimeStamp = TimeStamp,
			};
		}
	}
}
