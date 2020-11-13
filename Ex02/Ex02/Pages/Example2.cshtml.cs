using Microsoft.AspNetCore.Mvc.RazorPages;
using Ex02.Services.Interfaces;

namespace Ex02.Pages
{
    public class Example2Model : PageModel
    {
        readonly IUserService _userService;

        public Example2Model(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
            ViewData["allUsers"] = _userService.GetAll();
        }
    }
}
