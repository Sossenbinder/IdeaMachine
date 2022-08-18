using System;

namespace IdeaMachine.Modules.Account.Abstractions.DataTypes.Entity
{
	public class UserInfoEntity
	{
		public Guid UserId { get; set; }

		public string? ProfilePictureUrl { get; set; }
	}
}