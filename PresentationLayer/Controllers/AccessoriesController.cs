using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class AccessoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
