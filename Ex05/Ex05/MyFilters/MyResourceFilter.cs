using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ex05.MyFilters
{
    #region Синхронная версия фильтра
    public class MyResourceFilter : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            string userAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();

            if (Regex.IsMatch(userAgent, "MSIE | Trident"))
            {
                context.Result = new ContentResult
                {
                    Content = "Вы используете устаревший браузер.",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context) { }
    }
    #endregion

    #region Асинхронная версия фильтра
    public class MyResourceFilterAsync : Attribute, IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(
            ResourceExecutingContext context, 
            ResourceExecutionDelegate next)
        {
            string userAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();

            if (Regex.IsMatch(userAgent, "MSIE | Trident"))
            {
                context.Result = new ContentResult
                {
                    Content = "Вы используете устаревший браузер.",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            await next();
        }
    }
    #endregion
}
