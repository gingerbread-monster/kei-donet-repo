using Microsoft.AspNetCore.Mvc;
using System;
using Ex05.MyFilters;

namespace Ex05.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(MyExceptionFilter))]
    public class Example4Controller : ControllerBase
    {
        [HttpGet]
        [Route("get-data")]
        public IActionResult GetData()
        {
            throw new Exception("Демонстрация работы фильтра исключений.");

            return new JsonResult(new
            {
                Latitude = "37°37'46.4\"N",
                Longitude = "116°50'58.4\"W",
                IsGeographicalCoordinates = true
            });
        }
    }
}
