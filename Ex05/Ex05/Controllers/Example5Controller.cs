using Ex05.MyFilters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Ex05.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [MyResultFilter]
    public class Example5Controller : ControllerBase
    {
        [HttpGet]
        [Route("get-list")]
        public List<string> GetList()
        {
            return new();
        }

        [HttpGet]
        [Route("get-list2")]
        public List<string> GetList2()
        {
            return new()
            {
                "🟥 red",
                "🟧 orange",
                "🟨 yellow",
                "🟩 green",
                "🟦 blue",
                "🟪 violet",
                "⬜ white"
            };
        }
    }
}
