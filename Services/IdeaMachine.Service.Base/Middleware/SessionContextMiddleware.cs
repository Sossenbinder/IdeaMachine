using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Modules.Account.Abstractions.DataTypes;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Model;
using IdeaMachine.Modules.Account.Service.Interface;
using IdeaMachine.Modules.Session.Abstractions.DataTypes;
using IdeaMachine.Modules.Session.Service.Interface;
using IdeaMachine.Service.Base.Static;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace IdeaMachine.Service.Base.Middleware
{
	/// <summary>
	/// Middleware to initialize sessions based on the session cache
	/// </summary>
	public class SessionContextMiddleware : IMiddleware
	{
		public static string SessionContextIdentifier = "IdeaMachineSessionContext";

		private readonly ISessionService _sessionService;

		private readonly IAccountService _accountService;

		private const string AzureB2CIdentifier =
			"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

		private const string AzureB2CName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";

		public SessionContextMiddleware(
			ISessionService sessionService,
			IAccountService accountService)
		{
			_sessionService = sessionService;
			_accountService = accountService;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			await HandleRequest(context);
			await next(context);
		}

		private async Task HandleRequest(HttpContext context)
		{
			if (context.User.Identity?.IsAuthenticated ?? false)
			{
				var user = context.User.Claims.FirstOrDefault(x => x.Type == AzureB2CIdentifier)?.Value;

				if (user is not null && Guid.TryParse(user, out var userId))
				{
					var session = await InitializeSession(userId.ToString(),
						context.User.Claims.ToDictionary(x => x.Type, x => x.Value));
					context.Items[SessionContextIdentifier] = session;
					return;
				}
			}

			// It has to be an anonymous user then
			var isUserAnonymous = context.Request.Cookies.TryGetValue(IdentityDefinitions.AnonymousIdentification,
				out var anonCookieValue);
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

		private async Task<Session> InitializeSession(string userId, IReadOnlyDictionary<string, string> claims)
		{
			var parsedUserId = Guid.Parse(userId);

			var session = _sessionService.GetSession(parsedUserId);
			if (session is not null)
			{
				return session;
			}

			session = await ParseJwtToSession(parsedUserId, claims);
			await _sessionService.AddSession(parsedUserId, session);
			return session;
		}

		private async Task<Session> ParseJwtToSession(Guid userId, IReadOnlyDictionary<string, string> claims)
		{
			var email = claims["emails"];
			var userName = claims[AzureB2CName];

			return new Session()
			{
				User = new Account()
				{
					UserId = userId,
					Email = email,
					UserName = userName,
					ProfilePictureUrl = (await _accountService.GetProfilePictureUrl(new GetProfilePictureUrl(userId)))
						.PayloadOrNull,
				}
			};
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