using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
