using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ex02.Pages
{
    public class Example1Model : PageModel
    {
        MyAppEnvironmentService _myAppEnvironmentService;

        public Example1Model(MyAppEnvironmentService myAppEnvironmentService)
        {
            _myAppEnvironmentService = myAppEnvironmentService;
        }

        public void OnGet()
        {
            ViewData["appEnvironmentDescription"] = 
                _myAppEnvironmentService.GetAppEnvironmentDescription();
        }
    }
}
