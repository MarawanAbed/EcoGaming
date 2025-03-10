using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class AccessoriesItemsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
