using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Ex01.Middleware
{
    /// <summary>
    /// Middleware компонент который сопоставляет запрашиваемый маршрут
    /// с его обработчиком.
    /// </summary>
    public class RoutingMiddleware
    {
        public RoutingMiddleware(RequestDelegate next) { }

        public async Task InvokeAsync(HttpContext context)
        {
            string requestedPath = context.Request.Path.Value.ToLower();

            switch (requestedPath)
            {
                case "/index":
                    await context.Response.WriteAsync("You've accessed index page!");
                    break;
                case "/about":
                    await context.Response.WriteAsync("You've accessed the 'about' page!");
                    break;
                default:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    break;
            }
        }
    }
}
