using System;
using IdeaMachine.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachine.Controllers
{
	[Route("Identity")]
	public class IdentityController : IdentityControllerBase
	{
		[HttpGet]
		[Route("Identify")]
		public IActionResult Identify()
		{
			if (SessionOrNull is not null)
			{
				return Ok();
			}

			var expiration = DateTimeOffset.UtcNow.AddYears(100);

			var cookieOptions = new CookieOptions()
			{
				Expires = expiration,
				MaxAge = expiration.Subtract(DateTimeOffset.UtcNow),
			};

			Response.Cookies.Append(IdentityDefinitions.AnonymousIdentification, Guid.NewGuid().ToString(), cookieOptions);

			return Ok();
		}
	}
}