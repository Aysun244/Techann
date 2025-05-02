using Microsoft.AspNetCore.Mvc;

namespace Techan.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
