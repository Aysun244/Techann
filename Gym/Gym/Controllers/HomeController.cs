using Microsoft.AspNetCore.Mvc;

namespace Gym.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
