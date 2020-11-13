using Microsoft.AspNetCore.Mvc.RazorPages;
using Ex02.Services;

namespace Ex02.Pages
{
    public class Example3Model : PageModel
    {
        readonly SingletonService _singletonService;

        public int IterationsNumber;
        public string[] Result;

        public Example3Model(SingletonService singletonService)
        {
            _singletonService = singletonService;
        }

        public void OnGet(string lifetime)
        {
            IterationsNumber = 5;

            if (lifetime == "transient")
            {
                Result = new string[IterationsNumber];

                for (int i = 0; i < IterationsNumber; i++)
                {
                    var requestedService =
                    (TransientService) HttpContext.RequestServices.GetService(typeof(TransientService));

                    Result[i] = $"сервис: {nameof(TransientService)}, " +
                        $"обращение к сервису №: {i}, " +
                        $"значение: {requestedService.Id}";
                }
            }
            else if (lifetime == "scoped")
            {
                Result = new string[IterationsNumber];

                for (int i = 0; i < IterationsNumber; i++)
                {
                    var requestedService =
                    (ScopedService) HttpContext.RequestServices.GetService(typeof(ScopedService));

                    Result[i] = $"сервис: {nameof(ScopedService)}, " +
                        $"обращение к сервису №: {i}, " +
                        $"значение: {requestedService.Id}";
                }
            }
            else if (lifetime == "singleton")
            {
                Result = new string[IterationsNumber];

                for (int i = 0; i < IterationsNumber; i++)
                {
                    Result[i] = $"сервис: {nameof(SingletonService)}, " +
                        $"обращение к сервису №: {i}, " +
                        $"значение: {_singletonService.Id}";
                }
            }
        }
    }
}
