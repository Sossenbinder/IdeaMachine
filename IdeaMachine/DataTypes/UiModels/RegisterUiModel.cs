namespace IdeaMachine.DataTypes.UiModels
{
	public class RegisterUiModel
	{
		public string Email { get; set; } = null!;

		public string UserName { get; set; } = null!;

		public string Password { get; set; } = null!;

		public string ConfirmPassword { get; set; } = null!;
	}
}