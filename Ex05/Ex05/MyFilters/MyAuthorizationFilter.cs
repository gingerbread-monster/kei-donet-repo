using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ex05.MyFilters
{

    #region Синхронная версия фильтра
    public class MyAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool headerContainsKey = context.HttpContext.Request.Headers.ContainsKey("let-me-in");

            if (!headerContainsKey)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
        }
    }
    #endregion

    #region Асинхронная версия фильтра
    public class MyAuthorizationFilterAsync : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            bool headerContainsKey = context.HttpContext.Request.Headers.ContainsKey("let-me-in");

            if (!headerContainsKey)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
        }
    }
    #endregion
}
