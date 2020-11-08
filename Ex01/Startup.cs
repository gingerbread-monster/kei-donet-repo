using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ex01.Middleware;

namespace Ex01
{
    public class Startup
    {
        readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            #region 01: Самый простой middleware компонент

            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync(
            //        "Text response from terminal middleware component");
            //});

            #endregion

            #region 02: Последовательная работа нескольких middleware компонентов
            //app.Use(async (context, next) =>
            //{
            //    Debug.WriteLine(
            //        "[USE] doing some work before calling next middleware component");

            //    await next.Invoke();

            //    Debug.WriteLine("[USE] working after next component finished it's work");
            //});

            //app.Run(async context =>
            //{
            //    Debug.WriteLine("terminal middleware is running");

            //    await context.Response.WriteAsync(
            //        "Text response from terminal middleware component");
            //});
            #endregion

            #region 03: Разветвление middleware маршрутами
            //app.Map("/map1", appBuilder =>
            //    {
            //        appBuilder.Run(async context =>
            //        {
            //            await context.Response.WriteAsync("Mapped path 1");
            //        });
            //    });

            //void SecondMapHandler(IApplicationBuilder app)
            //{
            //    app.Run(async context =>
            //    {
            //        await context.Response.WriteAsync("Mapped path 2");
            //    });
            //};
            //app.Map("/map2", SecondMapHandler);

            //app.Map("/map3/route", appBuilder =>
            //{
            //    appBuilder.Run(async context =>
            //    {
            //        await context.Response.WriteAsync("Mapped path 3: multiple segments");
            //    });
            //});

            //app.Map("/map4", appBuilder =>
            //{
            //    appBuilder.Map("/map5", appBuilder =>
            //    {
            //        appBuilder.Run(async context =>
            //        {
            //            await context.Response.WriteAsync("Mapped path 4 and 5: nested mappings");
            //        });
            //    });
            //});

            //app.MapWhen(
            //    context => context.Request.Query.ContainsKey("param"),
            //    appBuilder =>
            //    {
            //        appBuilder.Run(async context =>
            //        {
            //            string paramValue = context.Request.Query["param"];
            //            await context.Response.WriteAsync(
            //                $"Path mapped when query key has value.\nParam value: {paramValue}");
            //        });
            //    });
            #endregion

            #region 04: Жизненный цикл middleware
            //int x = 1;

            //app.Run(async context =>
            //{
            //    x += 1;

            //    await context.Response.WriteAsync(x.ToString());
            //});
            #endregion

            #region 05: Использование пользовательского middleware класса
            //string correctPassword = "1111";

            //app.UseMiddleware<PasswordMiddleware>(correctPassword);

            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("You're authorized!");
            //});
            #endregion

        }
    }
}
