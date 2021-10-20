using System;

namespace IdeaMachineWeb.DataTypes.UiModels.Account
{
	public class UserUiModel
	{
		public bool IsAnonymous { get; set; }

		public Guid UserId { get; set; }

		public string UserName { get; set; } = null!;

		public string Email { get; set; } = null!;

		public DateTime LastAccessedAt { get; set; }
	}
}