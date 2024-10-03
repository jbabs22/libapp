using Microsoft.AspNetCore.Mvc;

namespace LibraryBookApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Terms()
        {
            return View();
        }
    }
}
