using System;
using System.Collections.Generic;
using System.Linq;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Modules.Idea.DataTypes.Entity;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;

namespace IdeaMachine.Modules.Idea.DataTypes.Model
{
	public class IdeaModel
	{
		public int Id { get; set; } = 0;

		public string ShortDescription { get; set; } = null!;

		public string? LongDescription { get; set; }

		public DateTime CreationDate { get; set; }

		public Guid CreatorId { get; set; }

		public List<string> Tags { get; set; } = new();

		public List<AttachmentModel> Attachments { get; set; } = new();

		public IdeaEntity ToEntity(ISession session)
		{
			return new()
			{
				CreatorId = session.User.UserId,
				CreationDate = CreationDate,
				LongDescription = LongDescription,
				ShortDescription = ShortDescription,
				Tags = Tags.Select(x => new TagEntity()
				{
					Tag = x,
				}).ToList(),
			};
		}

		public bool Validate()
		{
			return !ShortDescription.IsNullOrEmpty() || !LongDescription.IsNullOrEmpty() || Tags.Count <= 5;
		}
	}
}