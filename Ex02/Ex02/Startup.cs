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

        // ������ ����� ������������ ��� ���������� �������� � ��������� ��������.
        public void ConfigureServices(IServiceCollection services)
        {
            // ���������� ���������� �������� ��� ��������� Razor Pages.
            services.AddRazorPages();

            // 01: ����� ������� ��������� ������
            services.AddTransient<MyAppEnvironmentService>();

            // 02: ����������� �� ���������� �������
            services.AddTransient<IUserService, UserService>();

            // 03: ��������� ���� ��������
            services.AddDifferentLifetimeDemoServices();

            // 04: ��������� ���������� ���������� �������
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
