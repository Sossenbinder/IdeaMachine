using System;
using System.Threading.Tasks;
using IdeaMachine.Modules.Account.Abstractions.DataTypes;
using IdeaMachine.Modules.Session.Abstractions.DataTypes;
using IdeaMachine.Modules.Session.Service.Interface;
using IdeaMachineWeb.Static;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace IdeaMachineWeb.Middleware
{
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
			// Try to initialize a known session
			if (Guid.TryParse(context.User.Identity?.Name, out var userId))
			{
				var session = TryInitializeKnownSession(userId.ToString());
				if (session is not null)
				{
					context.Items[SessionContextIdentifier] = session;
					return;
				}
			}

			// TODO: Properly fix the scheme setup here

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