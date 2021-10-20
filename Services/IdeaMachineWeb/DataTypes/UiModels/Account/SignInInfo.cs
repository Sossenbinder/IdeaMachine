namespace IdeaMachineWeb.DataTypes.UiModels.Account
{
	public class SignInInfo
	{
		public string EmailUserName { get; set; } = null!;

		public string Password { get; set; } = null!;

		public bool RememberMe { get; set; }
	}
}