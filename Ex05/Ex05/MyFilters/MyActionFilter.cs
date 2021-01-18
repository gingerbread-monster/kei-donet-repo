using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace Ex05.MyFilters
{
    #region Синхронная версия фильтра
    public class MyActionFilter : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = new BadRequestObjectResult(context.ModelState);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Response.Headers.Add("my-action-filter-name", nameof(MyActionFilter));
        }
    }
    #endregion

    #region Асинхронная версия фильтра
    class MyActionFilterAsync : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
                context.Result = new BadRequestObjectResult(context.ModelState);

            await next();

            context.HttpContext.Response.Headers.Add("my-action-filter-name", nameof(MyActionFilterAsync));
        }
    }
    #endregion
}
