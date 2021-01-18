using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace Ex05.MyFilters
{
    public class MyExceptionFilter : Attribute, IExceptionFilter
    {
        readonly ILogger _logger;

        public MyExceptionFilter(ILogger<MyExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            string actionName = context.ActionDescriptor.DisplayName;
            string exceptionMessage = context.Exception.Message;

            context.Result = new JsonResult(new
            {
                ExceptionOccured = true,
                ExceptionOccuredAt = actionName,
                ExceptionMessage = exceptionMessage
            });

            _logger.LogInformation($"Exception occured at: {actionName}.\nException message: {exceptionMessage}");

            context.ExceptionHandled = true;
        }
    }
}
