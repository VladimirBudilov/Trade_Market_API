using Business.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace WebApi.Filters
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class ExceptionsFilterAttribute : Attribute, IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            var action = context?.ActionDescriptor.DisplayName;
            var callStack = context.Exception.StackTrace;
            var exceptionMessage = context.Exception.Message;
            var StatusCode = 500;

            if (context.Exception is MarketException)
            {
                StatusCode = 400;
            }
            else if (context.Exception is not null)
            {
                StatusCode = 404;
            }

            context.Result = new ContentResult
            {
                Content = $"Calling {action} failed, because: {exceptionMessage}. Callstack: {callStack}.",
                StatusCode = StatusCode,
            };

            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
