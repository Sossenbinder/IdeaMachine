using System;
using IdeaMachine.Modules.Idea.DataTypes.Entity;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;

namespace IdeaMachine.Modules.Idea.DataTypes
{
	public class IdeaModel
	{
		public int Id { get; set; } = 0;

		public string ShortDescription { get; set; } = null!;

		public string? LongDescription { get; set; }

		public DateTime CreationDate { get; set; }

		public IdeaEntity ToEntity(IUserSession session)
		{
			return new()
			{
				Creator = session.UserId,
				CreationDate = CreationDate,
				LongDescription = LongDescription,
				ShortDescription = ShortDescription,
			};
		}
	}
}