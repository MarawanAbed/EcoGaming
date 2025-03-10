using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
