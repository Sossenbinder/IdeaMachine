using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IdeaMachine.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View("~/Views/Index.cshtml");
		}
	}
}