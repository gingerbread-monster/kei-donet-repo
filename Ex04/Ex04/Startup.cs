using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Ex04.DataAccess.Example1;
using Ex04.DataAccess.Example2;
using Ex04.DataAccess.Example3;
using Ex04.Services.Interfaces;
using Ex04.Services.Implementations;
using Ex04.Extensions;

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

            #region 01: ����� ������� ������
            string connectionString1 = Configuration.GetSection("ConnectionStrings")["Example1"];

            services.AddDbContext<Example1DbContext>(options =>
            {
                options.UseSqlServer(connectionString1);
            });
            #endregion

            #region 02: "Code first" � ��������
            services.AddDbContext<Example2DbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:Example2"]);
            });
            #endregion

            #region 03: ������ � ����������
            string connectionString3 = Configuration.GetConnectionString("Example3");

            services.AddDbContext<Example3DbContext>(options =>
            {
                options.UseSqlServer(connectionString3);
            });

            services.AddTransient<ITaskService, TaskService>();

            services.ConfigureAutoMapper();
            #endregion
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
