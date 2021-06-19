using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace IdeaMachine.Attributes
{
	public sealed class CookieAuthorizeAttribute : AuthorizeAttribute
	{
		public CookieAuthorizeAttribute()
			: base(CookieAuthenticationDefaults.AuthenticationScheme)
		{
		}
	}
}