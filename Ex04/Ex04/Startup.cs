using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

using Ex04.DataAccess.Example1;
using Ex04.DataAccess.Example2;
using Ex04.DataAccess.Example3;
using Ex04.DataAccess.Example3.Entities;
using Ex04.Dtos;
using Ex04.Services.Helpers;
using Ex04.Services.Interfaces;
using Ex04.Services.Implementations;

namespace Ex04
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            #region 01: Самый простой пример
            //string connectionString = Configuration["DbConnectionStrings:Example1"];

            //services.AddDbContext<Example1DbContext>(options =>
            //{
            //    options.UseSqlServer(connectionString);
            //});
            #endregion

            #region 02: "Code first" и миграции
            services.AddDbContext<Example2DbContext>(options =>
            {
                options.UseSqlServer(Configuration["DbConnectionStrings:Example2"]);
            });
            #endregion

            #region 03: Пример с пагинацией
            //services.AddDbContext<Example3DbContext>(options =>
            //{
            //    options.UseSqlServer(Configuration["DbConnectionStrings:Example3"]);
            //});

            //services.AddTransient<ITaskService, TaskService>();
            #endregion
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
