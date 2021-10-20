using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using IdeaMachine.Modules.Account.Abstractions.DataTypes;
using IdeaMachine.Modules.Session.Abstractions.DataTypes;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;
using IdeaMachine.Modules.Session.Service.Interface;
using IdeaMachineWeb.Static;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdeaMachineWeb.Controllers
{
	public abstract class IdentityControllerBase : Controller
	{
		private readonly ISessionService _sessionService;

		protected ISession? SessionOrNull { get; private set; }

		protected ISession Session
		{
			get
			{
				if (SessionOrNull is null)
				{
					throw new AuthenticationException("No authenticated user found");
				}

				return SessionOrNull;
			}
		}

		protected Guid? UserIdOrNull { get; private set; }

		protected Guid UserId
		{
			get
			{
				if (UserIdOrNull is null)
				{
					throw new AuthenticationException("No authenticated user found");
				}

				return UserIdOrNull.Value;
			}
		}

		protected IdentityControllerBase(ISessionService sessionService)
		{
			_sessionService = sessionService;
		}

		public override async Task OnActionExecutionAsync(
			ActionExecutingContext context,
			ActionExecutionDelegate next)
		{
			await InitUserContext(context);

			await base.OnActionExecutionAsync(context, next);
		}

		private Task InitUserContext(ActionContext context)
		{
			if (Guid.TryParse(context.HttpContext.User.Identity?.Name, out var userId))
			{
				if (TryInitializeKnownSession(userId.ToString()))
				{
					return Task.CompletedTask;
				}
			}

			var isUserAnonymous = Request.Cookies.TryGetValue(IdentityDefinitions.AnonymousIdentification, out var anonCookieValue);
			if (!isUserAnonymous)
			{
				return Task.CompletedTask;
			}

			SessionOrNull = new Session()
			{
				User = new AnonymousUser()
				{
					UserName = "Anonymous user",
					UserId = Guid.Parse(anonCookieValue!),
					LastAccessedAt = DateTime.UtcNow,
				},
			};

			UserIdOrNull = SessionOrNull.User.UserId;

			return Task.CompletedTask;
		}

		private bool TryInitializeKnownSession(string userId)
		{
			UserIdOrNull = Guid.Parse(userId);

			var loggedInUser = _sessionService.GetSession(UserId);

			if (loggedInUser is null)
			{
				return false;
			}

			SessionOrNull = loggedInUser;
			return true;
		}
	}
}