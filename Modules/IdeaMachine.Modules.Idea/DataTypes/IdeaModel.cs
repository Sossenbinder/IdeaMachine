using System;

namespace IdeaMachine.Modules.Idea.DataTypes
{
	public class IdeaModel
	{
		public int Id { get; set; } = 0;

		public string ShortDescription { get; set; } = null!;

		public string? LongDescription { get; set; }

		public DateTime CreationDate { get; set; }

		public string? CreatorMail { get; set; }
	}
}