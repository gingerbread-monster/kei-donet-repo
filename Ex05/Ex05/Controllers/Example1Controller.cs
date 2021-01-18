using Microsoft.AspNetCore.Mvc;
using Ex05.MyFilters;

namespace Ex05.Controllers
{
    public class Example1Controller : Controller
    {
        [RequireHttps]
        public IActionResult UseBuiltInHttpsFilter()
        {
            return View(viewName: "Example1View", model: "Использование встроенного фильтра авторизации.");
        }

        [MyAuthorizationFilter]
        public IActionResult UseCustomAuthFilter()
        {
            return View(viewName: "Example1View", model: "Использование собственного созданного фильтра авторизации.");
        }
    }
}
