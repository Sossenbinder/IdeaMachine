using System.ComponentModel.DataAnnotations;

namespace IdeaMachine.DataTypes.UiModels.Account
{
	public class SignInInfo
	{
		[EmailAddress]
		public string EmailUserName { get; set; } = null!;

		public string Password { get; set; } = null!;

		public bool RememberMe { get; set; }
	}
}