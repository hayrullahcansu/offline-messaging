using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OfflineMessaging.Exceptions;
using OfflineMessaging.Services;

namespace OfflineMessaging.Attributes
{
    public class ExceptionHandlerFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var result = new ServiceResult();

            var exception = context.Exception;
            if (exception == null)
            {
                // Should never happen.
                return;
            }
            else if (exception is ApplicationException)
            {
                context.HttpContext.Response.StatusCode = 200;
                result.AddError(string.Empty, exception.Message);
            }
            else if (exception is UnauthorizedAccessException)
            {
                context.HttpContext.Response.StatusCode = 401;
                result.AddError(string.Empty, "Unauthorized Request");
            }
            else if (exception is NotFoundException)
            {
                context.HttpContext.Response.StatusCode = 404;
                result.AddError(string.Empty, exception.Message);
            }
            else if (exception is PayloadLargeException)
            {
                context.HttpContext.Response.StatusCode = 413;
                result.AddError(string.Empty, exception.Message);
            }
            else
            {
                //TODO: handle else
                context.HttpContext.Response.StatusCode = 200;
                result.AddError(string.Empty, exception.Message);
            }

            Console.WriteLine("--------------Exception------------");
            Console.WriteLine(exception.Message);
            Console.WriteLine(exception.StackTrace);
            Console.WriteLine(exception?.InnerException?.Message);

            context.Result = new ObjectResult(result);
        }
    }
}