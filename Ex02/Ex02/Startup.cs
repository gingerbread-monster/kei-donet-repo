using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ex02.Services.Interfaces;
using Ex02.Services;

namespace Ex02
{
    public class Startup
    {
        readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Данный метод используется для добавления сервисов в коллекцию сервисов.
        public void ConfigureServices(IServiceCollection services)
        {
            // Добавление встроенных сервисов для поддержки Razor Pages.
            services.AddRazorPages();

            // 01: Самый простой созданный сервис
            services.AddTransient<MyAppEnvironmentService>();

            // 02: Зависимость от интерфейса сервиса
            services.AddTransient<IUserService, UserService>();

            // 03: Жизненный цикл сервисов
            services.AddDifferentLifetimeDemoServices();

            // 04: Несколько реализаций интерфейса сервиса
            services.AddUserNotificationServices(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
