using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;

namespace Ex05.MyFilters
{
    public class MyResultFilter : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var objResult = context.Result as ObjectResult;
            var list = objResult.Value as List<string>;

            if (list?.FirstOrDefault() is null)
            {
                context.Result = new ContentResult
                {
                    Content = "Список пуст.",
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
        }
    }
}
