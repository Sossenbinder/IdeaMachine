﻿using System;

namespace IdeaMachine.Modules.Account.Abstractions.DataTypes.Model
{
	public record GetAccountNameRequest(Guid UserIdentifier);

	public record GetProfilePictureUrl(Guid UserIdentifier);
}