using System;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;

namespace IdeaMachine.Modules.Account.Abstractions.DataTypes.Model
{
	public class AccountModel : IUserSession
	{
		public bool IsAnonymous { get; } = false;

		public int Id { get; set; }

		public string UserName { get; set; } = null!;
		public Guid UserId { get; }
		public string Email { get; set; } = null!;

		public DateTime LastAccessedAt { get; set; }
	}
}