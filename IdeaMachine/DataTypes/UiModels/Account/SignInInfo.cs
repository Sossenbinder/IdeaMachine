using System.ComponentModel.DataAnnotations;

namespace IdeaMachine.DataTypes.UiModels.Account
{
	public class SignInInfo
	{
		[Required]
		[EmailAddress]
		public string EmailUserName { get; set; } = null!;

		[Required]
		public string Password { get; set; } = null!;

		public bool RememberMe { get; set; }
	}
}