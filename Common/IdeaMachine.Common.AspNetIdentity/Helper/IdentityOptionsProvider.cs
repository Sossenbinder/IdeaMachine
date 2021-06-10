using Microsoft.AspNetCore.Identity;

namespace IdeaMachine.Common.AspNetIdentity.Helper
{
	public static class IdentityOptionsProvider
	{
		public static void ApplyDefaultOptions(IdentityOptions options)
		{
			options.Password.RequireNonAlphanumeric = false;
			options.Password.RequireUppercase = false;

			options.SignIn.RequireConfirmedEmail = true;
			options.User.RequireUniqueEmail = true;
		}
	}
}