using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace IdeaMachine.Modules.Account.DataTypes.Entity
{
	public class AccountEntity : IdentityUser<int>
	{
		[Column("LastAccessedAt")]
		public DateTime LastAccessedAt { get; set; }

		// ReSharper disable once NotNullMemberIsNotInitialized
		public AccountEntity()
		{
		}
	}
}