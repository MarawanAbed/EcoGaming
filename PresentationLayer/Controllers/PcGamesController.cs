using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class PcGamesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
