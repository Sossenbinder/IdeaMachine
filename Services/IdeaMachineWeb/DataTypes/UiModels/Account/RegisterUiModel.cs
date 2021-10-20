using System.ComponentModel.DataAnnotations;

namespace IdeaMachine.DataTypes.UiModels.Account
{
	public class RegisterUiModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; } = null!;

		[Required]
		public string UserName { get; set; } = null!;

		[Required]
		[Compare(nameof(ConfirmPassword))]
		public string Password { get; set; } = null!;

		[Required]
		[Compare(nameof(Password))]
		public string ConfirmPassword { get; set; } = null!;
	}
}