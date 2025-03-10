using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
