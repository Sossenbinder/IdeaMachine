﻿using System;

namespace IdeaMachine.Modules.Reaction.DataTypes.Entity
{
	public class ReactionEntity
	{
		public Guid UserId { get; set; }

		public int IdeaId { get; set; }

		public LikeState LikeState { get; set; }
	}
}