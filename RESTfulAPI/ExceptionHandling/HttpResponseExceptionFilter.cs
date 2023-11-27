using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace RESTfulAPI.ExceptionHandling
{
    public class HttpResponseExceptionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // The IActionFilter interface requires these two methods declared, but we don't need this one so left it empty.
            // We avoid throwing a NotImplemented exception.
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Prevents ASP.NET Core default unhandled exception handler to be called.
            if (context.Exception is Exception exception)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.HttpContext.Response.ContentType = "application/json";

                var errorPayload = new ErrorDetails()
                {
                    ErrorCode = context.HttpContext.Response.StatusCode,
                    ErrorMessage = exception.ToString()
                };

                context.Result = new ObjectResult(errorPayload);

                context.ExceptionHandled = true;
            }
        }
    }
}
