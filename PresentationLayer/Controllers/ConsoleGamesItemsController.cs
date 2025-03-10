using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class ConsoleGamesItemsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
