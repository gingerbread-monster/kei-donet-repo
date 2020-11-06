using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ex01.Pages
{
    public class IndexModel : PageModel
    {
        public string[] People;

        public string message1, message2;

        public void OnGet(string msg1, string msg2)
        {
            People = new string[] { "Вася", "Петя", "Олег", "Коля" };

            message1 = msg1;
            message2 = msg2;
        }

        public void OnPost(int? num1, int? num2)
        {
            if (num1 is null)
                num1 = default(int);

            if (num2 is null)
                num2 = default(int);

            ViewData["sum"] = num1 + num2;
        }
    }
}
