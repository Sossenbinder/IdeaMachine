using System.ComponentModel.DataAnnotations;

namespace IdeaMachine.DataTypes.UiModels.Account
{
	public class VerifyEmailUiModel
	{
		[Required]
		public string UserName { get; set; } = null!;

		[Required]
		public string Token { get; set; } = null!;

		public void Deconstruct(out string userName, out string token)
		{
			userName = UserName;
			token = Token;
		}
	}
}