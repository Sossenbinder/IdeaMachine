using System;
using System.Collections.Generic;
using System.Linq;
using IdeaMachine.Modules.Idea.DataTypes.Model;

namespace IdeaMachine.Modules.Idea.DataTypes.Entity
{
	public class IdeaEntity
	{
		public int Id { get; set; } = 0;

		public Guid CreatorId { get; set; }

		public string ShortDescription { get; set; } = null!;

		public string? LongDescription { get; set; }

		public DateTime CreationDate { get; set; }

		public List<TagEntity>? Tags { get; set; }

		public List<AttachmentEntity>? AttachmentUrls { get; set; }

		public List<CommentEntity>? Comments { get; set; }

		public IdeaModel ToModel()
		{
			return new()
			{
				CreationDate = CreationDate,
				CreatorId = CreatorId,
				ShortDescription = ShortDescription,
				LongDescription = LongDescription,
				Id = Id,
				Tags = Tags?.Select(x => x.Tag).ToList() ?? new List<string>(),
				Attachments = AttachmentUrls?.Select(x => x.ToModel()).ToList() ?? new List<AttachmentModel>()
			};
		}
	}
}