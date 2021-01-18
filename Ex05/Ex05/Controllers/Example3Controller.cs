using Microsoft.AspNetCore.Mvc;
using Ex05.Models;
using Ex05.MyFilters;

namespace Ex05.Controllers
{
    public class Example3Controller : Controller
    {

        public IActionResult SubmitPerson()
        {
            return View();
        }

        [MyActionFilter]
        public IActionResult ViewSubmittedPerson(PersonModel person)
        {
            return View(person);
        }
    }
}
