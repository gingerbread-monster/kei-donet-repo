using Microsoft.AspNetCore.Mvc;

namespace Ex05.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
