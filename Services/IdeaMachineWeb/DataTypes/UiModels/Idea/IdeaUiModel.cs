﻿using System;
using System.Collections.Generic;
using IdeaMachine.Modules.Idea.Abstractions.DataTypes.Model;
using IdeaMachine.Modules.Idea.DataTypes.Model;

namespace IdeaMachineWeb.DataTypes.UiModels.Idea
{
	public class IdeaUiModel
	{
		public int Id { get; set; } = 0;

		public string ShortDescription { get; set; } = null!;

		public string? LongDescription { get; set; }

		public DateTime CreationDate { get; set; }

		public Guid CreatorId { get; set; }

		public IdeaReactionMetaData IdeaReactionMetaData { get; set; } = null!;

		public List<string> Tags { get; set; } = null!;

		public List<AttachmentModel> Attachments { get; set; } = null!;

		public static IdeaUiModel FromModel(IdeaModel model)
		{
			return new()
			{
				CreationDate = model.CreationDate,
				CreatorId = model.CreatorId,
				Id = model.Id,
				LongDescription = model.LongDescription,
				ShortDescription = model.ShortDescription,
				Tags = model.Tags,
				Attachments = model.Attachments,
			};
		}
	}
}