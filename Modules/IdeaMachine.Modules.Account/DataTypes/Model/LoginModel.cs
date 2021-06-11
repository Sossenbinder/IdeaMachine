namespace IdeaMachine.Modules.Account.DataTypes.Model
{
	public class LoginModel
	{
		public string EmailUserName { get; set; } = null!;

		public string Password { get; set; } = null!;

		public bool RememberMe { get; set; }
	}
}