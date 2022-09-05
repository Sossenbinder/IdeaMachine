using System;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Modules.Session.Abstractions.DataTypes;
using IdeaMachine.Modules.Session.Abstractions.DataTypes.Interface;
using IdeaMachine.Service.Base.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdeaMachine.Service.Base.Controller
{
	public abstract class IdentityControllerBase : Microsoft.AspNetCore.Mvc.Controller
	{
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

		public override async Task OnActionExecutionAsync(
			ActionExecutingContext context,
			ActionExecutionDelegate next)
		{
			InitUserContext(context);

			await base.OnActionExecutionAsync(context, next);
		}

		private void InitUserContext(ActionContext context)
		{
			if (!context.HttpContext.Items.TryGetValueTyped(SessionContextMiddleware.SessionContextIdentifier, out Session? session))
			{
				return;
			}

			SessionOrNull = session!;
			UserIdOrNull = session!.User.UserId;
		}

		protected IActionResult InternalServerError()
		{
			return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
		}
	}
}