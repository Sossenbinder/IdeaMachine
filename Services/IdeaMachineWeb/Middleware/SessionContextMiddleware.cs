using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdeaMachine.Modules.Account.Abstractions.DataTypes;
using IdeaMachine.Modules.Session.Abstractions.DataTypes;
using IdeaMachine.Modules.Session.Service.Interface;
using IdeaMachineWeb.Static;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace IdeaMachineWeb.Middleware
{
	/// <summary>
	/// Middleware to initialize sessions based on the session cache
	/// </summary>
	public class SessionContextMiddleware : IMiddleware
	{
		public static string SessionContextIdentifier = "IdeaMachineSessionContext";

		private readonly ISessionService _sessionService;

		public SessionContextMiddleware(ISessionService sessionService)
		{
			_sessionService = sessionService;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			HandleRequest(context);
			await next(context);
		}

		private void HandleRequest(HttpContext context)
		{
			if (context.User.Identity?.IsAuthenticated ?? false)
			{
				string? name = null;
				var authenticationMethod = context.User.Claims.Any(x => x.Type == ClaimTypes.AuthenticationMethod);

				if (!authenticationMethod)
				{
					// Try to initialize a known username/password session
					name = context.User.Identity?.Name;
				}
				else
				{
					// Has to be an external scheme then
					if (context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.AuthenticationMethod)?.Value == GoogleDefaults.AuthenticationScheme)
					{
						name = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
					}
				}

				if (name is not null && Guid.TryParse(name, out var userId))
				{
					var session = TryInitializeKnownSession(userId.ToString());
					if (session is not null)
					{
						context.Items[SessionContextIdentifier] = session;
						return;
					}
				}

				// TODO: Properly fix the scheme setup here
			}

			// It has to be an anonymous user then
			var isUserAnonymous = context.Request.Cookies.TryGetValue(IdentityDefinitions.AnonymousIdentification, out var anonCookieValue);
			if (!isUserAnonymous)
			{
				return;
			}

			var anonymousSession = new Session()
			{
				User = new AnonymousUser()
				{
					UserName = "Anonymous user",
					UserId = Guid.Parse(anonCookieValue!),
					LastAccessedAt = DateTime.UtcNow,
				},
			};

			context.Items[SessionContextIdentifier] = anonymousSession;
		}

		private Session? TryInitializeKnownSession(string userId)
		{
			var parsedUserId = Guid.Parse(userId);

			return _sessionService.GetSession(parsedUserId);
		}
	}

	public static class SessionContextMiddlewareExtensions
	{
		public static IApplicationBuilder UseSessionContextMiddleware(this IApplicationBuilder app)
		{
			return app.UseMiddleware<SessionContextMiddleware>();
		}
	}
}