using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
namespace HBlog.Infrastructure.Middlewares
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
                ArgumentException argumentException => (400, argumentException.Message),
                BadHttpRequestException badrequestException => (400, badrequestException.Message),
                _ => (500, "Internal server error.")
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = errorMsg,
                Type = exception.GetType().Name,
                Detail = exception.Message
            };

            _logger.LogError(exception, exception.Message);
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
            return true;

        }
    }
}
