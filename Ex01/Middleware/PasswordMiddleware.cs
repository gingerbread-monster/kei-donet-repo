using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Ex01.Middleware
{
    /// <summary>
    /// Middleware компонент который проверяет пароль передаваемый
    /// пользователем через параметр <i>password</i> адресной строки запроса.
    /// </summary>
    public class PasswordMiddleware
    {
        RequestDelegate nextMiddlewareComponent;

        string _validPassword;

        public PasswordMiddleware(
            RequestDelegate next,
            string validPassword)
        {
            nextMiddlewareComponent = next;
            _validPassword = validPassword;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string passwordToCheck = context.Request.Query["password"];

            if (passwordToCheck != _validPassword)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;

                await context.Response.WriteAsync("Wrong password!");
            }
            else
            {
                await nextMiddlewareComponent.Invoke(context);
            }
        }
    }
}
