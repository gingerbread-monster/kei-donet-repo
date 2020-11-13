using System;
using Microsoft.AspNetCore.Hosting;

namespace Ex02
{
    public class MyAppEnvironmentService
    {
        readonly IWebHostEnvironment _environment;

        public MyAppEnvironmentService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string GetAppEnvironmentDescription()
        {
            string toNewLine = Environment.NewLine;

            return $"Имя приложения : {_environment.ApplicationName}{toNewLine}" +
                $"Имя окружения : {_environment.EnvironmentName}{toNewLine}" +
                $"Путь к корню приложения : {_environment.ContentRootPath}{toNewLine}" +
                $"Путь к корню веб-содержимого приложения : {_environment.WebRootPath ?? "Каталог отсутствует"}";
        }
    }
}
