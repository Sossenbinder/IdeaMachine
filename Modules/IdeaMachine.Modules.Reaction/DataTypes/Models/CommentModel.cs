using System;

namespace IdeaMachine.Modules.Reaction.DataTypes.Models
{
	public class CommentModel
	{
		public Guid CommenterId {  get; set; }

		public int IdeaId { get; set; }

		public string Comment { get; set; }
	}
}
