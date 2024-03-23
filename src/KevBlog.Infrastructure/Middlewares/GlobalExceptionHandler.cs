using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
namespace KevBlog.Infrastructure.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public ILogger<GlobalExceptionHandler> _logger { get; }
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }


        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            (int statusCode, string errorMsg) = exception switch
            {
                //=> (403, null),
                BadHttpRequestException badrequestException => (400, badrequestException.Message),
                _ => (500, "Internal server error.")
            };
            _logger.LogError(exception, exception.Message);
            await httpContext.Response.WriteAsJsonAsync(errorMsg);
            return true;

        }
    }
}
