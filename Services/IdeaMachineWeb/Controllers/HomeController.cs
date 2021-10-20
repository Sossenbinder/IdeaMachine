using System;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Modules.Account.DataTypes.Model;
using IdeaMachine.Modules.Account.Service.Interface;
using IdeaMachine.Modules.Session.Abstractions.DataTypes;
using IdeaMachine.Modules.Session.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachine.Controllers
{
	public class HomeController : IdentityControllerBase
	{
		private readonly ILoginService _loginService;

		public HomeController(
			ISessionService sessionService,
			ILoginService loginService)
			: base(sessionService)
		{
			_loginService = loginService;
		}

		public async Task<IActionResult> Index()
		{
			if ((HttpContext.User.Identity?.IsAuthenticated ?? false) && (SessionOrNull?.IsAnonymous ?? false))
			{
				await _loginService.RefreshLogin(new RefreshLoginModel()
				{
					UserId = Guid.Parse(HttpContext.User.Claims.First().Value),
				});
			}

			return View("~/Views/Index.cshtml");
		}
	}
}