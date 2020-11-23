using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ex02.Services;
using Ex02.Services.Interfaces;
using Ex02.Models;

namespace Ex02
{
    public static class Extensions
    {
        /// <summary>
        /// Метод расширения добавляющий сервисы для примера с демонстрацией
        /// различных lifetime опций для зависимостей (сервисов).
        /// </summary>
        /// <param name="services">Объект представляющий коллекцию сервисов.</param>
        public static void AddDifferentLifetimeDemoServices(this IServiceCollection services)
        {
            services.AddTransient<TransientService>();
            services.AddScoped<ScopedService>();
            services.AddSingleton<SingletonService>();
        }

        /// <summary>
        /// Метод расширения добавляющий сервисы для оповещения пользователя.
        /// </summary>
        /// <param name="services">Объект представляющий коллекцию сервисов.</param>
        public static void AddUserNotificationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IUserNotifierService, EmailNotifierService>();
            services.AddTransient<IUserNotifierService, MailNotifierService>();

            services.Configure<UserNotificationSettings>(
                configuration.GetSection("UserNotificationSettings"));

            #region Альтернативный способ динамического выбора реализации c помощью "фабрики реализаций"
            //services.AddTransient<IUserNotifierService>(provider =>
            //{
            //    int currentCentury = DateTime.Now.Year / 100 + 1;
            //    if (currentCentury < 21)
            //    {
            //        return new MailNotifierService();
            //    }
            //    else
            //    {
            //        return new EmailNotifierService();
            //    }
            //});
            #endregion
        }
    }
}
