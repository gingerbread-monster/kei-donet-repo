using Microsoft.AspNetCore.Mvc;
using Ex05.MyFilters;

namespace Ex05.Controllers
{
    [MyResourceFilter]
    public class Example2Controller : Controller
    {
        public IActionResult Action1()
        {
            return View(viewName: "Index", model: "action 1");
        }

        public IActionResult Action2()
        {
            return View(viewName: "Index", model: "action 2");
        }

        public IActionResult Action3()
        {
            return View(viewName: "Index", model: "action 3");
        }
    }
}
