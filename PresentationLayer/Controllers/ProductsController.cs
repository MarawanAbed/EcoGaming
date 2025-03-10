using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
