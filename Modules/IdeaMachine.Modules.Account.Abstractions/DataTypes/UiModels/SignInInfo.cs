namespace IdeaMachine.Modules.Account.Abstractions.DataTypes.UiModels
{
	public class SignInInfo
	{
		public string EmailUserName { get; set; } = null!;

		public string Password { get; set; } = null!;

		public bool RememberMe { get; set; }
	}
}