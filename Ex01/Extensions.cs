using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ex01.Middleware;

namespace Ex01
{
    /// <summary>
    /// Статический класс определяющий методы расширения для приложения.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Метод расширения который:
        /// <list type="number">
        /// <item>считывает настройки JWT для приложения;</item>
        /// <item>привязывает эти значения к классу <see cref="AppJwtData"/>;</item>
        /// <item>добавляет объект <see cref="AppJwtData"/> со значениями как сервис,
        /// в коллекцию сервисов приложения.</item>
        /// </list>
        /// </summary>
        public static void BindAppJwtData(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var appJwtData = new AppJwtData();

            configuration.GetSection("AppJwtData").Bind(appJwtData);

            services.AddSingleton(appJwtData);
        }

        /// <summary>
        /// Метод расширения который добавляет в конвейер
        /// использование <see cref="JwtAuthenticationMiddleware"/>.
        /// </summary>
        public static IApplicationBuilder UseJwtAuthentication(
            this IApplicationBuilder app)
        {
            return app.UseMiddleware<JwtAuthenticationMiddleware>();
        }
    }
}
