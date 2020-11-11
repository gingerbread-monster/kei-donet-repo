using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Ex01.Middleware
{
    /// <summary>
    /// Middleware компонент который отвечает за обработку ошибок.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next.Invoke(context);

            switch (context.Response.StatusCode)
            {
                case StatusCodes.Status400BadRequest:
                    await context.Response.WriteAsync("Error: Invalid JWT.");
                    break;
                case StatusCodes.Status401Unauthorized:
                    await context.Response.WriteAsync("Error: There's no JWT in the request header.");
                    break;
                case StatusCodes.Status404NotFound:
                    await context.Response.WriteAsync("Error: Page not found.");
                    break;
                default:
                    break;
            }
        }
    }
}
