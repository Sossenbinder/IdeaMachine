using System;
using IdeaMachine.Service.Base.Controller;
using IdeaMachine.Service.Base.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachineWeb.Controllers
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