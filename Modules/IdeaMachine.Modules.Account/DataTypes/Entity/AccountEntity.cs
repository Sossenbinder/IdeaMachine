﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace IdeaMachine.Modules.Account.DataTypes.Entity
{
	public class AccountEntity : IdentityUser<Guid>
	{
		[Column("LastAccessedAt")]
		public DateTime LastAccessedAt { get; set; }

		// ReSharper disable once NotNullMemberIsNotInitialized
		public AccountEntity()
		{
		}

		public Abstractions.DataTypes.Account ToModel()
		{
			return new()
			{
				UserId = Id,
				Email = Email,
				UserName = UserName,
				LastAccessedAt = LastAccessedAt,
			};
		}
	}
}