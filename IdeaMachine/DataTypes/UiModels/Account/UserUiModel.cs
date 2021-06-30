using System;

namespace IdeaMachine.DataTypes.UiModels.Account
{
	public class UserUiModel
	{
		public bool IsAnonymous { get; set; }

		public Guid UserId { get; set; }

		public string UserName { get; set; }

		public string Email { get; set; }

		public DateTime LastAccessedAt { get; set; }
	}
}