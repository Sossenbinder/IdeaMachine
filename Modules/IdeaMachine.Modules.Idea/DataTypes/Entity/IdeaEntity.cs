using System;
using System.ComponentModel.DataAnnotations.Schema;
using IdeaMachine.Modules.Account.DataTypes.Entity;

namespace IdeaMachine.Modules.Idea.DataTypes.Entity
{
	public class IdeaEntity
	{
		public int Id { get; set; } = 0;

		public Guid Creator { get; set; }

		[ForeignKey(nameof(Creator))]
		public AccountEntity? Account { get; set; }

		public string ShortDescription { get; set; } = null!;

		public string? LongDescription { get; set; }

		public DateTime CreationDate { get; set; }

		public IdeaModel ToModel()
		{
			return new()
			{
				CreationDate = CreationDate,
				CreatorId = Creator,
				ShortDescription = ShortDescription,
				LongDescription = LongDescription,
				Id = Id,
			};
		}
	}
}