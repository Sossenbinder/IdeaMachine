using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Authentication;
using System.Threading.Tasks;
using IdeaMachine.Modules.Session.DataTypes;
using IdeaMachine.Modules.Session.DataTypes.Interface;
using IdeaMachine.Static;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdeaMachine.Controllers
{
	public abstract class IdentityControllerBase : Controller
	{
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

		public override async Task OnActionExecutionAsync(
			[NotNull] ActionExecutingContext context,
			ActionExecutionDelegate next)
		{
			await InitUserContext();

			await base.OnActionExecutionAsync(context, next);
		}

		private Task InitUserContext()
		{
			// Init regular context here

			var isUserAnonymous = Request.Cookies.TryGetValue(IdentityDefinitions.AnonymousIdentification, out var anonCookieValue);
			if (isUserAnonymous)
			{
				SessionOrNull = new AnonymousUserSession(Guid.Parse(anonCookieValue!));
			}

			return Task.CompletedTask;
		}
	}
}