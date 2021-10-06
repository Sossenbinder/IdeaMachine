using System;
using IdeaMachine.Modules.Idea.DataTypes.Model;

namespace IdeaMachine.Modules.Idea.DataTypes.UiModel
{
	public class CommentUiModel
	{
		public int Id { get; set; }

		public Guid CommenterId { get; set; }

		public int IdeaId { get; set; }

		public string Comment { get; set; } = null!;

		public DateTime TimeStamp { get; set; }

		public void Deconstruct(out int id, out Guid commenterId, out int ideaId, out string comment, out DateTime timeStamp)
		{
			id = Id;
			commenterId = CommenterId;
			ideaId = IdeaId;
			comment = Comment;
			timeStamp = TimeStamp;
		}

		public static CommentUiModel FromModel(CommentModel model)
		{
			return new()
			{
				Id = model.CommentId,
				IdeaId = model.IdeaId,
				Comment = model.Comment,
				CommenterId = model.CommenterId,
				TimeStamp = model.TimeStamp,
			};
		}
	}
}
