using System;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using IdeaMachine.Modules.Session.Abstractions.DataTypes;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;
using IdeaMachine.Modules.Session.Service.Interface;
using IdeaMachine.Static;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdeaMachine.Controllers
{
	public abstract class IdentityControllerBase : Controller
	{
		private readonly ISessionService _sessionService;

		protected IUserSession? SessionOrNull { get; private set; }

		protected IUserSession Session
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

		private Task InitUserContext(ActionExecutingContext context)
		{
			// Init regular context here
			var userId = context.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

			if (userId is not null)
			{
				if (TryInitializeKnownSession(userId.Value))
				{
					return Task.CompletedTask;
				}
			}

			var isUserAnonymous = Request.Cookies.TryGetValue(IdentityDefinitions.AnonymousIdentification, out var anonCookieValue);
			if (isUserAnonymous)
			{
				SessionOrNull = new AnonymousUserSession(Guid.Parse(anonCookieValue!));
			}

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