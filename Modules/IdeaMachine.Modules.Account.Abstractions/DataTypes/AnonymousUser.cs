using System;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Interface;

namespace IdeaMachine.Modules.Account.Abstractions.DataTypes
{
	public class AnonymousUser : IUser
	{
		public Guid UserId { get; set; }

		public string UserName { get; set; } = "";

		public string Email { get; set; } = "";

		public DateTime LastAccessedAt { get; set; } = DateTime.UtcNow;
	}
}